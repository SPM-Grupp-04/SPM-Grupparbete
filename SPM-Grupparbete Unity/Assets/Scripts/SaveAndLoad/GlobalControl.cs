using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public bool IsSceneBeingLoaded = false;
    // Variabler som ska sparas över scener.

    public PlayerStatistics playerStatistics;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
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


        formatter.Serialize(saveFile, Instance.playerStatistics);
        saveFile.Close();
    }

    public void LoadData()
    {


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);

        playerStatistics = formatter.Deserialize(saveFile) as PlayerStatistics;
        saveFile.Close();
    }
}