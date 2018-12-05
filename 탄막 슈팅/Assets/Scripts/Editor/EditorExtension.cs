using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorExtension
{

    public static void AddToArray<T>(ref T[] list, T elementToAdd)
    {
        int length = list.Length;
        T[] newList = new T[length + 1];
        for (int i = 0; i < length; i++)
        {
            newList[i] = list[i];
        }
        newList[length] = elementToAdd;
        list = newList;
    }

    public static void DeleteFromArray<T>(ref T[] list, int indexToDelete)
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