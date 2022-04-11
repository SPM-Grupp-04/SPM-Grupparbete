using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EgilGlobalControl : MonoBehaviour
{
    public static EgilGlobalControl Instance;

    public bool IsSceneBeingLoaded = false;
    // Variabler som ska sparas över scener.

    public PlayerStatistics SavedData = new PlayerStatistics();
    
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void SaveData()
    {
        if (!Directory.Exists("Saves"))
        {
            Directory.CreateDirectory("Saves");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");

        SavedData = Instance.SavedData;
        
        formatter.Serialize(saveFile, SavedData);
        saveFile.Close();
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);

        SavedData = (PlayerStatistics) formatter.Deserialize(saveFile);
        saveFile.Close();
    }
    
}
