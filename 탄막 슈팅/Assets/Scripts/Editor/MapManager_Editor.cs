using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(MapManager))]
public class MapManager_Editor : Editor {

    MapManager manager;

    int selectedRegionIndex = -1;
    int selectedMonIndex = -1;

    [SerializeField] Color[] monsterColors;
    int monsterSpeciesCnt;
    const float SIZE = .5f;
    const float SQUARED_SIZE = .25f;
    const string savingName = "MapManager_Data";


    private void OnEnable()
    {
        manager = target as MapManager;
        int currentSpeciesCnt = Enum.GetNames(typeof(MonsterName)).Length;
        monsterColors = new Color[monsterSpeciesCnt];

        string data = EditorPrefs.GetString("MapManager_Data", JsonUtility.ToJson(this));
        JsonUtility.FromJsonOverwrite(data, this);

        if (monsterSpeciesCnt != currentSpeciesCnt)
        {
            int diff = currentSpeciesCnt - monsterSpeciesCnt;
            monsterSpeciesCnt = currentSpeciesCnt;

            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    EditorExtension.AddToArray<Color>(ref monsterColors, Color.white);
                }
            }
            else
            {
                for (int i = 0; i < diff; i++)
                {
                    EditorExtension.DeleteFromArray<Color>(ref monsterColors, monsterColors.Length - 1);
                }
            }

        }
    }

    private void OnSceneGUI()
    {
        
        Vector2 mousePos = GetWorldMousePosition();

        bool leftMouseClick = false;
        bool rightMouseClick = false;
        if (Event.current.type == EventType.MouseDown)
        {
            if (Event.current.button == 0)
            {
                leftMouseClick = true;

                if (Event.current.shift) 
                {
                    if (selectedRegionIndex >= 0 && selectedRegionIndex < manager.regions.Length) // Add monster
                    {
                        Undo.RecordObject(manager, "Add monster");
                        EditorExtension.AddToArray<MonsterSpawnData>(ref manager.regions[selectedRegionIndex].monsters, new MonsterSpawnData(mousePos, 0));
                        Select(selectedRegionIndex, manager.regions[selectedRegionIndex].monsters.Length - 1);
                    }
                    else // Add region
                    {
                        Undo.RecordObject(manager, "Add region");
                        EditorExtension.AddToArray<Region>(ref manager.regions, new Region(/*"Region " + (manager.regions.Length + 1).ToString(), */mousePos + Vector2.one, mousePos - Vector2.one));
                        Select(manager.regions.Length - 1, -1);
                    }
                }
                else // Clicking empty area
                {
                    selectedRegionIndex = -1;
                    selectedMonIndex = -1;
                }
            }
            else if (Event.current.button == 1)
                rightMouseClick = true;
        }

        for (int r = 0; r < manager.regions.Length; r++)
        {
            Region region = manager.regions[r];

            // monsters
            for (int m = 0; m < region.monsters.Length; m++)
            {
                // draw monsters
                if (r == selectedRegionIndex && m == selectedMonIndex)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = monsterColors[(int)region.monsters[m].name];
                }

                manager.Handle(ref region.monsters[m].spawnPoint, SIZE, "Move monster");

                // handle click
                if ((region.monsters[m].spawnPoint - mousePos).sqrMagnitude < SQUARED_SIZE)
                {
                    if (leftMouseClick)
                    {
                        Select(r, m);
                    }
                    else if (rightMouseClick) // delete the monster
                    {
                        Undo.RecordObject(manager, "Delete monster");
                        EditorExtension.DeleteFromArray<MonsterSpawnData>(ref manager.regions[r].monsters, m);
                    }
                }
            }

            // regions
            Vector2 UR = region.upperRight;
            Vector2 LL = region.lowerLeft;

            if ((region.upperRight - mousePos).sqrMagnitude < SQUARED_SIZE || 
                (region.lowerLeft - mousePos).sqrMagnitude < SQUARED_SIZE)
            {
                if (leftMouseClick) // select the region
                {
                    Select(r, -1);
                }
                if (rightMouseClick) // delete the region
                {
                    Undo.RecordObject(manager, "Delete region");
                    EditorExtension.DeleteFromArray<Region>(ref manager.regions, r);
                    continue;
                }
            }

            // draw camera rect
            if (UR.x < LL.x || UR.y < LL.y)
                Handles.color = Color.cyan;
            else
                Handles.color = selectedRegionIndex == r ? Color.red : Color.yellow;

            manager.Handle(ref manager.regions[r].lowerLeft, SIZE, "Change camera rect");
            manager.Handle(ref manager.regions[r].upperRight, SIZE, "Change camera rect");

            Handles.DrawPolyLine(new Vector3[]
            {
                region.upperRight,
                new Vector3(region.upperRight.x, region.lowerLeft.y),
                region.lowerLeft,
                new Vector3(region.lowerLeft.x, region.upperRight.y),
                region.upperRight
            });
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //if (GUILayout.Button("Save"))
        //    this.Save(savingName);

        GUILayout.Space(10);
        GUILayout.Label("Color to display monsters");

        for (int i = 0; i < monsterSpeciesCnt; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Enum.GetName(typeof(MonsterName), i));

            Color newColor = EditorGUILayout.ColorField(monsterColors[i]);
            if (monsterColors[i] != newColor)
            {
                monsterColors[i] = newColor;
                this.Save(savingName);
            }

            GUILayout.EndHorizontal();
        }

        if (selectedRegionIndex >= 0 &&
            selectedRegionIndex < manager.regions.Length &&
            selectedMonIndex >= 0 &&
            selectedMonIndex < manager.regions[selectedRegionIndex].monsters.Length)
        {
            GUILayout.Space(10);
            GUILayout.Label("Species of selected monster");

            MonsterSpawnData[] monsterList = manager.regions[selectedRegionIndex].monsters;
            monsterList[selectedMonIndex].name = (MonsterName)EditorGUILayout.EnumPopup(monsterList[selectedMonIndex].name);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
            EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        }
    }

    Vector2 GetWorldMousePosition()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
        //mousePosition.y = -mousePosition.y;
        return mousePosition;
    }

    void Select(int region, int monster)
    {
        selectedRegionIndex = region;
        selectedMonIndex = monster;
        Repaint();
    }
}