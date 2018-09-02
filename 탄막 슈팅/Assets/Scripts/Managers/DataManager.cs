using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Text;
using System;

public static class Data
{
    public static int fileIndex;

    public static void Save<T>(T data, string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        bf.Serialize(file, data);
        file.Close();
    }

    public static bool Load<T>(string filePath, out T result)
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(filePath))
        {
            FileStream file = File.Open(filePath, FileMode.Open);
            T t = (T)bf.Deserialize(file);
            file.Close();
            result = t;
            return true;
        }

        result = default(T);
        return false;
    }

    public static string DeathPath(string bossName)
    {
        return ConvertValid("is" + bossName + "Dead");
    }

    public static string ConvertValid(string partialPath)
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