using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Portal))]
public class Portal_Editor : Editor {

    Portal portal;
    string regionName;

    const float size = .5f;

    private void OnEnable()
    {
        portal = (Portal)target;
        SetRegionName();
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label(regionName);
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
            SetRegionName();
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.green;
        portal.Handle(ref portal.goal, size, "Move goal", target.name);
    }

    void SetRegionName()
    {
        regionName = portal.regionNumber >= 0 ?
            MapManager.Instance.regions[portal.regionNumber].regionName :
            MapManager.Instance.scenes[-portal.regionNumber - 1];
    }
}
