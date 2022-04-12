using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EgilHealth : MonoBehaviour
{
    public PlayerStatistics LocalPlayerData = PlayerStatistics.Instance;

    [SerializeField] private String playerName;


    private void Start()
    {
        if (playerName == "PlayerOne")
        {
            LocalPlayerData.PlayerOnehp = EgilGlobalControl.Instance.SavedData.PlayerOnehp;
        }
        
        if (playerName == "PlayerTwo")
        {
            LocalPlayerData.PlayerTwoHP = EgilGlobalControl.Instance.SavedData.PlayerTwoHP;
        }
    }

    void Update()
    {
        if (playerName == "PlayerOne")
        {
            if (LocalPlayerData.PlayerOnehp < 1)
            {
                Destroy(transform.gameObject);
            }
        }

        if (playerName == "PlayerTwo")
        {
            if (LocalPlayerData.PlayerTwoHP < 1)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void Takedamage()
    {
        if (playerName == "PlayerOne")
        {
            LocalPlayerData.PlayerOnehp--;
        }
        else
        {
            LocalPlayerData.PlayerTwoHP--;
        }

        SavePlayer();
    }

    public void SavePlayer()
    {
        EgilGlobalControl.Instance.SavedData = LocalPlayerData;
    }
}