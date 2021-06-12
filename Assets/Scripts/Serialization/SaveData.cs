using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private static SaveData _saveData;
    public static SaveData saveData {
        get {
            if(_saveData == null) _saveData = new SaveData();
            return _saveData;
        }
    }
    public int save_variable_1;

}
