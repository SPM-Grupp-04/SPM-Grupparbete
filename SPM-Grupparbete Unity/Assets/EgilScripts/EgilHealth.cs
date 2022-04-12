using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class EgilHealth : MonoBehaviour
{
    [FormerlySerializedAs("LocalPlayerData")] public EgilPlayerStatistics localEgilPlayerData = EgilPlayerStatistics.Instance;

    [SerializeField] private String playerName;


    private void Start()
    {
        if (playerName == "PlayerOne")
        {
            localEgilPlayerData.PlayerOnehp = EgilGlobalControl.Instance.SavedData.PlayerOnehp;
        }
        
        if (playerName == "PlayerTwo")
        {
            localEgilPlayerData.PlayerTwoHP = EgilGlobalControl.Instance.SavedData.PlayerTwoHP;
        }
    }

    void Update()
    {
        if (playerName == "PlayerOne")
        {
            if (localEgilPlayerData.PlayerOnehp < 1)
            {
                Destroy(transform.gameObject);
            }
        }

        if (playerName == "PlayerTwo")
        {
            if (localEgilPlayerData.PlayerTwoHP < 1)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void Takedamage()
    {
        if (playerName == "PlayerOne")
        {
            localEgilPlayerData.PlayerOnehp--;
        }
        else
        {
            localEgilPlayerData.PlayerTwoHP--;
        }

        SavePlayer();
    }

    public void GainCrystal()
    {
        if (playerName == "PlayerOne")
        {
            localEgilPlayerData.Crystals++;
        }
        else
        {
            localEgilPlayerData.Crystals++;
        }
    }
    public void SavePlayer()
    {
        EgilGlobalControl.Instance.SavedData = localEgilPlayerData;
    }
}