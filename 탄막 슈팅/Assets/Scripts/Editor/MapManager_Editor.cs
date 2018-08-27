using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MapManager))]
public class MapManager_Editor : Editor {

    MapManager manager;

    int selectedRegionIndex = -1;
    int selectedMonIndex = -1;

    [SerializeField] Color[] monsterColors;
    int monsterSpeciesCnt;
    const float size = .5f;
    const string savingName = "MapManager_Data";


    private void OnEnable()
    {
        manager = (MapManager)target;
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
                    AddToArray<Color>(ref monsterColors, Color.white);
                }
            }
            else
            {
                for (int i = 0; i < diff; i++)
                {
                    DeleteFromArray<Color>(ref monsterColors, monsterColors.Length - 1);
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
                        AddToArray<MonsterSpawnData>(ref manager.regions[selectedRegionIndex].monsters, new MonsterSpawnData(mousePos, 0));
                        Select(selectedRegionIndex, manager.regions[selectedRegionIndex].monsters.Length - 1);
                    }
                    else // Add region
                    {
                        Undo.RecordObject(manager, "Add region");
                        AddToArray<Region>(ref manager.regions, new Region("Region " + (manager.regions.Length + 1).ToString(), mousePos + Vector2.one, mousePos - Vector2.one));
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
                    Handles.color = Color.yellow;
                }
                else
                {
                    Handles.color = monsterColors[(int)region.monsters[m].name];
                }

                manager.Handle(ref region.monsters[m].spawnPoint, size, "Move monster", savingName);

                // handle click
                if (GameManager.IsNear(region.monsters[m].spawnPoint, mousePos, size))
                {
                    if (leftMouseClick)
                    {
                        Select(r, m);
                    }
                    else if (rightMouseClick) // delete the monster
                    {
                        Undo.RecordObject(manager, "Delete monster");
                        DeleteFromArray<MonsterSpawnData>(ref manager.regions[r].monsters, m);
                    }
                }
            }

            // regions
            Vector2 UR = region.upperRight;
            Vector2 LL = region.lowerLeft;

            if ((GameManager.IsNear(region.upperRight, mousePos, size) || // delete region
                GameManager.IsNear(region.lowerLeft, mousePos, size)) &&
                rightMouseClick)
            {
                Undo.RecordObject(manager, "Delete region");
                DeleteFromArray<Region>(ref manager.regions, r);
                continue;
            }

            if ((GameManager.IsNear(region.upperRight, mousePos, size) ||
                GameManager.IsNear(region.lowerLeft, mousePos, size))
                && leftMouseClick)
            {
                Select(r, -1);
            }

            // draw camera rect
            if (UR.x < LL.x || UR.y < LL.y)
                Handles.color = Color.cyan;
            else
                Handles.color = selectedRegionIndex == r ? Color.yellow : Color.red;

            manager.Handle(ref manager.regions[r].lowerLeft, size, "Change camera rect", savingName);
            manager.Handle(ref manager.regions[r].upperRight, size, "Change camera rect", savingName);

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

        if (GUILayout.Button("Save"))
            this.Save(savingName);

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
    }

    void AddToArray<T>(ref T[] list, T elementToAdd)
    {
        int length = list.Length;
        T[] newList = new T[length+ 1];
        for (int i = 0; i < length; i++)
        {
            newList[i] = list[i];
        }
        newList[length] = elementToAdd;
        list = newList;
    }

    void DeleteFromArray<T>(ref T[] list, int indexToDelete)
    {
        T[] newList = new T[list.Length - 1];
        int newListIndex = 0;

        for (int i = 0; i < list.Length; i++)
        {
            if (i != indexToDelete)
            {
                newList[newListIndex] = list[i];
                newListIndex++;
            }
        }
        list = newList;
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
    /*
    MapManager mapManager;

    List<MonsterSpawnData> monsterList;
    int currentRegionNum = 0;
    int selectedIndex;

    const float size = .5f;

    private void OnEnable()
    {
        mapManager = (MapManager)target;
        if (mapManager.regions == null)
        {
            mapManager.regions = new Region[1];
            monsterList = new List<MonsterSpawnData>() { new MonsterSpawnData() };
            selectedIndex = 0;
            SaveResult();
        }
        else
        {
            ChangeRegion();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        return;

        // 지역 선택
        int newRegionNum = EditorGUILayout.IntSlider(currentRegionNum, 0, mapManager.regions.Length - 1);
        if (currentRegionNum != newRegionNum)
        {
            currentRegionNum = newRegionNum;
            ChangeRegion();
        }

        GUILayout.Space(30);

        // 지역 입장 트리거 설정



        GUILayout.Space(30);

        // 몬스터 종 설정
        MonsterSpawnData selected = monsterList[selectedIndex];
        monsterList[selectedIndex] = new MonsterSpawnData(selected.spawnPoint, (MonsterName)EditorGUILayout.EnumPopup(selected.name));

        // 몬스터 추가 및 초기화
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Monster"))
        {
            AddMonster();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Reset"))
        {
            monsterList = new List<MonsterSpawnData>() { new MonsterSpawnData() };
            SaveResult();
        }
        GUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        //DrawMonsters();
    }

    void DrawMonsters()
    {
        Handles.color = Color.red;
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (i == selectedIndex)
                Handles.color = Color.cyan;
            else
                Handles.color = Color.blue;

            Vector2 newPos = Handles.FreeMoveHandle(monsterList[i].spawnPoint, Quaternion.identity, size, Vector2.one, Handles.CylinderHandleCap);
            if (monsterList[i].spawnPoint != newPos)
            {
                selectedIndex = i;
                monsterList[i] = new MonsterSpawnData(newPos, monsterList[i].name);
                SaveResult();
            }
        }
    }

    void ChangeRegion()
    {
        selectedIndex = 0;
        monsterList = new List<MonsterSpawnData>(mapManager.regions[currentRegionNum].monsters);
        SaveResult();
    }

    void SaveResult()
    {
        mapManager.regions[currentRegionNum].monsters = new MonsterSpawnData[monsterList.Count];
        for (int i = 0; i < monsterList.Count; i++)
        {
            mapManager.regions[currentRegionNum].monsters[i] = monsterList[i];
        }
    }

    void AddMonster()
    {
        monsterList.Add(new MonsterSpawnData(Vector2.zero, 0)); // HandleUtility.GUIPointToWorldRay(new Vector2(Screen.width/2, Screen.height/2)).origin
    }
}
*/