using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationManager : MonoBehaviour
{

    public static bool save(string saveName, SaveData saveData){
        BinaryFormatter bf = getBinaryFormatter();

        if(!Directory.Exists(Application.persistentDataPath + "/saves")){
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        FileStream file = File.Create(Application.persistentDataPath + "/saves/" + saveName + ".knk");
        bf.Serialize(file, saveData);
        file.Close();
        return true;
    }

    public static SaveData load(string path){
        if(!File.Exists(path)) return null;

        BinaryFormatter bf = getBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        try {
            SaveData save = bf.Deserialize(file) as SaveData;
            file.Close();
            return save;
        } catch {
            Debug.LogError($"Failed to load file in path {path}");
            return null;
        }
    }
    public static BinaryFormatter getBinaryFormatter() => new BinaryFormatter();
}
