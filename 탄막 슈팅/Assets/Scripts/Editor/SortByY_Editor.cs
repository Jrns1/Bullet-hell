using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SortByY))]
public class SortByY_Editor : Editor {

    SortByY sorter;

    private void OnEnable()
    {
        sorter = target as SortByY;
    }

    private void OnSceneGUI()
    {
        Vector2 pos = sorter.transform.position + new Vector3(0, sorter.footOffset, 0);
        Handles.color = Color.green;
        EditorExtension.Handle(sorter, ref pos, .1f, "Modify foot", "SortByY");
        sorter.footOffset = pos.y - sorter.transform.position.y;
    }

}
