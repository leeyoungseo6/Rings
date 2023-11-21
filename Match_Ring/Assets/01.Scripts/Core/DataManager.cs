using System;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    public static DataManager Instance;

    string GameDataFileName = "/GameData.json";

    public Data Data;

    public void Init()
    {
        LoadGameData();
    }

    private void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<Data>(FromJsonData);
        }
    }
    
    private void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(Data, true);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus == false) SaveGameData();
    }
}