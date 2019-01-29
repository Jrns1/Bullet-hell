using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Text;
using System;

public class DataManager : Singleton<DataManager> {

    public int fileIndex;

    BinaryFormatter bf = new BinaryFormatter();
    StringBuilder sb = new StringBuilder();

    public void Save<T>(T data, string filePath)
    {
        FileStream file = File.Create(ConvertValid(filePath));
        bf.Serialize(file, data);
        file.Close();
    }

    public bool Load<T>(string filePath, out T result)
    {
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

    public bool IsDead(string bossName)
    {
        bool dead = false;
        Load<bool>(ConvertValid(bossName), out dead);
        return dead;
    }

    string ConvertValid(string partialPath)
    {
        sb.Append(Application.persistentDataPath);
        sb.Append("\\");
        sb.Append(fileIndex.ToString());
        sb.Append("\\");
        sb.Append(partialPath);
        sb.Append(".bin");

        return sb.ToString();
    }

}