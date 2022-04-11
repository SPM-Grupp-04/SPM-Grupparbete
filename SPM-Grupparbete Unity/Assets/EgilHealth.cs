using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EgilHealth : MonoBehaviour
{
    public PlayerStatistics LocalPlayerData = new PlayerStatistics();

    private void Start()
    {
        LocalPlayerData.hp = EgilGlobalControl.Instance.SavedData.hp;
    }

    void Update()
    {
        if (LocalPlayerData.hp < 1)
        {
            Destroy(transform.gameObject);
        }
    }

    public void Takedamage()
    {
        LocalPlayerData.hp--;
        SavePlayer();
    }

    public void SavePlayer()
    {
        EgilGlobalControl.Instance.SavedData = LocalPlayerData;
    }
}