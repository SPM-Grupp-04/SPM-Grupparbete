using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using EgilEventSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerState : MonoBehaviour, IDamagable
{
    private PlayerStatistics m_LocalPlayerData = PlayerStatistics.Instance;

    [SerializeField] private String playerName;
    private void Start()
    {
        if (playerName == "PlayerOne")
        {
            m_LocalPlayerData.PlayerOneHealth = GlobalControl.Instance.SavedData.PlayerOneHealth;
            m_LocalPlayerData.PlayerOneAcceleration = GlobalControl.Instance.SavedData.PlayerOneAcceleration;
            m_LocalPlayerData.PlayerOneDisco = GlobalControl.Instance.SavedData.PlayerOneDisco;
        }

        if (playerName == "PlayerTwo")
        {
            m_LocalPlayerData.PlayerTwoHealth = GlobalControl.Instance.SavedData.PlayerTwoHealth;
            m_LocalPlayerData.PlayerTwoAcceleration = GlobalControl.Instance.SavedData.PlayerTwoAcceleration;
            m_LocalPlayerData.PlayerTwoDisco = GlobalControl.Instance.SavedData.PlayerTwoDisco;
        }
    }

    void Update()
    {
        
        if (playerName == "PlayerOne")
        {
            if (m_LocalPlayerData.PlayerOneHealth < 1)
            {
                die(gameObject);
                
            }
        }

        if (playerName == "PlayerTwo")
        {
            if (m_LocalPlayerData.PlayerTwoHealth < 1)
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
            m_LocalPlayerData.PlayerOneHealth -= damage;
        }
        else
        {
            m_LocalPlayerData.PlayerTwoHealth -= damage;
        }

        SavePlayers();
    }

    public void Heal(int healAmount)
    {
        m_LocalPlayerData.PlayerOneHealth += healAmount;
        m_LocalPlayerData.PlayerTwoHealth += healAmount;

        SavePlayers();
    }

    public void SetAcceleration(float newAcceleration)
    {
        m_LocalPlayerData.PlayerOneAcceleration = newAcceleration;
        m_LocalPlayerData.PlayerTwoAcceleration = newAcceleration;

        SavePlayers();
    }

    public void SetDisco(bool isDisco)
    {
        m_LocalPlayerData.PlayerOneDisco = isDisco;
        m_LocalPlayerData.PlayerTwoDisco = isDisco;

        SavePlayers();
    }

    public void GainCrystal()
    {
        if (playerName == "PlayerOne")
        {
            m_LocalPlayerData.Crystals++;
        }
        else
        {
            m_LocalPlayerData.Crystals++;
        }
    }

    public void SavePlayers()
    {
        GlobalControl.Instance.SavedData = m_LocalPlayerData;
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