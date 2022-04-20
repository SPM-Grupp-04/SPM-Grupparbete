using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using EgilEventSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class EgilPlayerState : MonoBehaviour, IDamagable
{
    private EgilPlayerStatistics localEgilPlayerData = EgilPlayerStatistics.Instance;

    [SerializeField] private String playerName;
    private void Start()
    {
        if (playerName == "PlayerOne")
        {
            localEgilPlayerData.PlayerOneHealth = EgilGlobalControl.Instance.SavedData.PlayerOneHealth;
            localEgilPlayerData.PlayerOneAcceleration = EgilGlobalControl.Instance.SavedData.PlayerOneAcceleration;
            localEgilPlayerData.PlayerOneDisco = EgilGlobalControl.Instance.SavedData.PlayerOneDisco;
        }

        if (playerName == "PlayerTwo")
        {
            localEgilPlayerData.PlayerTwoHealth = EgilGlobalControl.Instance.SavedData.PlayerTwoHealth;
            localEgilPlayerData.PlayerTwoAcceleration = EgilGlobalControl.Instance.SavedData.PlayerTwoAcceleration;
            localEgilPlayerData.PlayerTwoDisco = EgilGlobalControl.Instance.SavedData.PlayerTwoDisco;
        }
    }

    void Update()
    {
        
        if (playerName == "PlayerOne")
        {
            if (localEgilPlayerData.PlayerOneHealth < 1)
            {
                die(gameObject);
                
            }
        }

        if (playerName == "PlayerTwo")
        {
            if (localEgilPlayerData.PlayerTwoHealth < 1)
            {
                die(gameObject);
               
            }
        }
    }

    void die(GameObject gameObject)
    {
        var dieEvent = new DieEvenInfo(gameObject);
        
        EventSystem.current.FireEvent(dieEvent);
    }
    

    public void DealDamage(int damage)
    {
        
        if (playerName == "PlayerOne")
        {
            localEgilPlayerData.PlayerOneHealth -= damage;
        }
        else
        {
            localEgilPlayerData.PlayerTwoHealth -= damage;
        }

        SavePlayers();
    }

    public void Heal(int healAmount)
    {
        localEgilPlayerData.PlayerOneHealth += healAmount;
        localEgilPlayerData.PlayerTwoHealth += healAmount;

        SavePlayers();
    }

    public void SetAcceleration(float newAcceleration)
    {
        localEgilPlayerData.PlayerOneAcceleration = newAcceleration;
        localEgilPlayerData.PlayerTwoAcceleration = newAcceleration;

        SavePlayers();
    }

    public void SetDisco(bool isDisco)
    {
        localEgilPlayerData.PlayerOneDisco = isDisco;
        localEgilPlayerData.PlayerTwoDisco = isDisco;

        SavePlayers();
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

    public void SavePlayers()
    {
        EgilGlobalControl.Instance.SavedData = localEgilPlayerData;
    }

    //using a bitmask
    public enum PlayerSelection
    {
        None = 0b_0000_0000, //0
        PlayerOne = 0b_0000_0001, //1
        PlayerTwo = 0b_0000_0010, //2
        Both = PlayerOne | PlayerTwo //3, stored as 0b_0000_0011
    }
}