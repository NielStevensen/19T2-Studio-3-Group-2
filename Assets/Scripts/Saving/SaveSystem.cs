using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{ 
    public static void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stats.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData save = new SaveData(data);
        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static SaveData LoadSave()
    {
        string path = Application.persistentDataPath + "/stats.sav";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData save =  formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return save;
        }
        else
        {
            Debug.Log("noSave");
            return new SaveData();
        }
    }
}