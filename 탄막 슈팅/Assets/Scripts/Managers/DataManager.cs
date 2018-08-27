using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Text;

public static class DataManager {

    public static int fileIndex;

    public static void Save<T>(T data, string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetFilePath(filePath));
        bf.Serialize(file, data);
        file.Close();
    }

    public static T Load<T>(string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(GetFilePath(filePath), FileMode.Open);
        T t = (T)bf.Deserialize(file);
        file.Close();

        return t;
    }

    public static string GetFilePath(string partialPath)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(Application.persistentDataPath);
        sb.Append("\\");
        sb.Append(fileIndex.ToString());
        sb.Append("\\");
        sb.Append(partialPath);
        sb.Append(".bin");

        return sb.ToString();
    }
}