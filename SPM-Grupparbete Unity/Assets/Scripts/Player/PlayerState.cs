using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using EgilEventSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerState : MonoBehaviour, IDamagable
{
    public PlayerStatistics m_LocalPlayerData;

    [SerializeField] private String playerName;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        m_LocalPlayerData = PlayerStatistics.Instance;
    }

    private void Start()
    {
        
        
      //  m_LocalPlayerData.Crystals = GlobalControl.Instance.playerStatistics.Crystals;
        m_LocalPlayerData.BlueCrystals = GlobalControl.Instance.playerStatistics.BlueCrystals;
        m_LocalPlayerData.RedCrystals = GlobalControl.Instance.playerStatistics.RedCrystals;
        m_LocalPlayerData.drillLevel = GlobalControl.Instance.playerStatistics.drillLevel;
        m_LocalPlayerData.componentsCollectedMask = GlobalControl.Instance.playerStatistics.componentsCollectedMask;
        m_LocalPlayerData.playerMaxHealth = GlobalControl.Instance.playerStatistics.playerMaxHealth;
      
        if (playerName == "PlayerOne")
        {
            m_LocalPlayerData.playerOneHealth = GlobalControl.Instance.playerStatistics.playerOneHealth;
            m_LocalPlayerData.playerOneAcceleration = GlobalControl.Instance.playerStatistics.playerOneAcceleration;
            m_LocalPlayerData.playerOneDisco = GlobalControl.Instance.playerStatistics.playerOneDisco;
            
        }
        
        if (playerName == "PlayerTwo")
        {
            m_LocalPlayerData.playerTwoHealth = GlobalControl.Instance.playerStatistics.playerTwoHealth;
            m_LocalPlayerData.playerTwoAcceleration = GlobalControl.Instance.playerStatistics.playerTwoAcceleration;
            m_LocalPlayerData.playerTwoDisco = GlobalControl.Instance.playerStatistics.playerTwoDisco;
        }

        if (m_LocalPlayerData.playerOneHealth <= 0)
        {
            m_LocalPlayerData.playerOneHealth = 1;
        }
        if (m_LocalPlayerData.playerTwoHealth <= 0)
        {
            m_LocalPlayerData.playerTwoHealth = 1;
        }
    }

    void Update()
    {
        if (playerName == "PlayerOne")
        {
            if (m_LocalPlayerData.playerOneHealth < 1)
            {

                StartCoroutine(WaitForAnimation(gameObject));
            }
        } 

        if (playerName == "PlayerTwo")
        {
            if (m_LocalPlayerData.playerTwoHealth < 1)
            {

                StartCoroutine(WaitForAnimation(gameObject));
            }
        }
    }

    IEnumerator WaitForAnimation(GameObject g)
    {
        animator.SetBool("IsDead",true);
        yield return new WaitForSeconds(2);
        die(g);
    }
    
    void die(GameObject gameObject)
    {
     
        
        
        var dieEvent = new DieEvenInfo(gameObject);

        EventSystem.current.FireEvent(dieEvent);

        
       

    }

    public void Heal()
    {
       

        m_LocalPlayerData.playerOneHealth = m_LocalPlayerData.playerMaxHealth;
        m_LocalPlayerData.playerTwoHealth = m_LocalPlayerData.playerMaxHealth;
        
        GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
        
    }

    public void DealDamage(float damage)
    {
        if (playerName == "PlayerOne")
        {
            m_LocalPlayerData.playerOneHealth -= damage;
        }
        else
        {
            m_LocalPlayerData.playerTwoHealth -= damage;
        }

        SavePlayers();
    }

    public void IncreaseMaxHealth(int maxHealthIncreaseAmount)
    {
        m_LocalPlayerData.playerMaxHealth += maxHealthIncreaseAmount;
        m_LocalPlayerData.playerOneHealth = m_LocalPlayerData.playerMaxHealth;
        m_LocalPlayerData.playerTwoHealth = m_LocalPlayerData.playerMaxHealth;

        SavePlayers();
    }

    public void SetAcceleration(float newAcceleration)
    {
        m_LocalPlayerData.playerOneAcceleration = newAcceleration;
        m_LocalPlayerData.playerTwoAcceleration = newAcceleration;

        SavePlayers();
    }

    
    
    public void SetDisco(bool isDisco)
    {
        m_LocalPlayerData.playerOneDisco = isDisco;
        m_LocalPlayerData.playerTwoDisco = isDisco;

        SavePlayers();
    }
    

    public void SavePlayers()
    {
        GlobalControl.Instance.playerStatistics = m_LocalPlayerData;
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