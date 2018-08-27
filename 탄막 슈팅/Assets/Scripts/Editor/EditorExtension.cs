using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorExtension
{
    public static void Handle(this MonoBehaviour mono, ref Vector2 pos, float size, string undoName, string saveName)
    {
        Vector2 newPos = Handles.FreeMoveHandle(pos, Quaternion.identity, size, Vector2.one, Handles.CylinderHandleCap);
        if (pos != newPos)
        {
            Undo.RecordObject(mono, undoName);
            pos = newPos;
        }
    }

    public static void Save(this Editor editor, string name)
    {
        EditorPrefs.SetString(name, JsonUtility.ToJson(editor));
    }
}