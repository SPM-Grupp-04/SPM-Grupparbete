//Main author: Axel Ingelsson Fredler
//Additional programming: Simon Canbäck, sica4801

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ShopScript : MonoBehaviour
{
    [SerializeField] private GameObject shopInterfaceBackground;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] [Range(1.0f, 10.0f)] private float shopAreaRadius = 5.0f;

    [SerializeField] private int drillLevelCostBlue = 5;
    [SerializeField] private int healCostBlue = 2;
    [SerializeField] private int weaponCostBlue = 5;
    [SerializeField] private int speedCostBlue = 5;
    [SerializeField] private int discoCostBlue = 5;
    
    private int drillLevelCostRed = 0;
    private Collider[] shopColliders;
    private SphereCollider shopCollider;
    private PlayerState m_PlayerState;
    bool pauseButtonPressed;
    private bool GameIsPause;

    private Dictionary<string, bool> buttonDictionary;
    [Header("Buttons for shop")]
    [SerializeField] private Button healButton;
    [SerializeField] private Button accelerateButton;
    [SerializeField] private Button discoButton;
    [SerializeField] private Button drillButton;
    [SerializeField] private Button weaponButton;
    
    
    
    
    private void Start()
    {
        Debug.Log(buttonDictionary);
        buttonDictionary = PlayerStatistics.Instance.buttonDictionary;
        
        if (buttonDictionary == null)
        {
            
            Debug.Log("I am empty");
            buttonDictionary = new Dictionary<string, bool>();
            for (int i = 0; i < shopInterfaceBackground.transform.childCount; i++)
            {
                Transform temp = shopInterfaceBackground.transform.GetChild(i);
                if (temp.gameObject.CompareTag("ShopButton"))
                {
                    string addedButton = shopInterfaceBackground.transform.GetChild(i).gameObject.name;
                    buttonDictionary.Add(addedButton, false);
                    temp.name = addedButton;
                }
            }
            Debug.Log("I am now full");
        }
        else
        {
            Debug.Log("Loaded BD form PS");
            
        }

        foreach (KeyValuePair<string, bool> test in buttonDictionary)
        {
            
            Debug.Log(test.Key + " " + test.Value.ToString());
        }
        
        shopInterfaceBackground.SetActive(false);
        shopCollider = GetComponent<SphereCollider>();
        m_PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        shopCollider.radius = shopAreaRadius;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        drillButton.Select();
        CanPlayersHeal();
        UpdateShop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        if (other.gameObject.GetComponent<PlayerController>().IsMapSwitched())
        {
            OpenShopInterface();
        }
        else
        {
            CloseShopInterface();
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        CloseShopInterface();
        
    }

    private void OpenShopInterface()
    {
        shopInterfaceBackground.SetActive(true);
    }
    
    private void CloseShopInterface()
    {
        PlayerStatistics.Instance.buttonDictionary = buttonDictionary;
        
        Debug.Log("Current BD ");
        foreach (KeyValuePair<string, bool> test in buttonDictionary)
        {
            
            Debug.Log(test.Key + " " + test.Value.ToString());
        }
        Debug.Log("------------------------------------------------");
        Debug.Log("Saved BD");
        foreach (KeyValuePair<string, bool> test in PlayerStatistics.Instance.buttonDictionary)
        {
            
            Debug.Log(test.Key + " " + test.Value.ToString());
        }
        shopInterfaceBackground.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shopAreaRadius);
    }
    
    public void Heal()
    {
        
            if (m_PlayerState.m_LocalPlayerData.BlueCrystals > healCostBlue)
            {
                m_PlayerState.Heal();
                m_PlayerState.m_LocalPlayerData.BlueCrystals -= healCostBlue;
                healButton.Select();
                healButton.interactable = false;
            }
    }

    public void DrillUpgrade(int level)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevelCostBlue)
        {
            drillButton.interactable = false;
            m_PlayerState.m_LocalPlayerData.drillLevel = level;
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= drillLevelCostBlue;
            GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
            drillButton.Select();
            buttonDictionary[drillButton.name] = true;
            UpdateShop();
        }
    }

    public void Accelerate(float addedAcceleration)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= speedCostBlue)
        {
            accelerateButton.interactable = false;
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerOneAcceleration + addedAcceleration);
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= speedCostBlue;
            buttonDictionary[accelerateButton.name] = true;
            accelerateButton.Select();
        }
    }

    public void Disco(bool isDisco)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= discoCostBlue)
        {
            m_PlayerState.SetDisco(isDisco);
            discoButton.interactable = false;
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= discoCostBlue;
            buttonDictionary[discoButton.name] = true;
            discoButton.Select();
        }
    }

    public void WeaponUpgrade(int level)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= weaponCostBlue)
        {
            m_PlayerState.m_LocalPlayerData.weaponLevel = level;
            weaponButton.interactable = false;
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= weaponCostBlue;
            buttonDictionary[weaponButton.name] = true;
            weaponButton.Select();
        }
    }

    private void CanPlayersHeal()
    {
        if (m_PlayerState.m_LocalPlayerData.playerOneHealth.Equals(m_PlayerState.m_LocalPlayerData.playerMaxHealth)
            && m_PlayerState.m_LocalPlayerData.playerTwoHealth.Equals(m_PlayerState.m_LocalPlayerData.playerMaxHealth))
        {
            healButton.interactable = false;
            return;   
        }

        healButton.interactable = true;
    }

    private void UpdateShop()
    {
        
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        if (buttonDictionary[drillButton.name] == false)
        {
            foreach (string buttonName in buttonDictionary.Keys)
            {
                if (!buttonName.Equals(drillButton.name))
                {
                    FindButton(buttonName).interactable = false;
                    
                }
            }

            
        }
        else
        {
            foreach (string buttonName in buttonDictionary.Keys)
            {
                if (buttonDictionary[buttonName] == false)
                {
                    FindButton(buttonName).interactable = true;
                    
                    if (buttonName.Equals(healButton.name))
                    {
                        CanPlayersHeal();
                        
                    }
                }
                else
                {
                    
                    FindButton(buttonName).interactable = false;
                    
                }
            }
        }
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
       
        Debug.Log("Run time: " + ts);
    }

    private Button FindButton(string buttonName)
    {
        Debug.Log(buttonName);
        GameObject temp = GameObject.Find("UI_Shop/ShopCanvas/ShopInterfaceBackground/" + buttonName);
        Button tempButton = temp.GetComponent<Button>();
        return tempButton;
    }
    
}