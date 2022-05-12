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
    
    private Dictionary<string, bool> buttonDictionary = new Dictionary<string, bool>();
    [Header("Buttons for shop")]
    [SerializeField] private Button healButton;
    [SerializeField] private Button accelerateButton;
    [SerializeField] private Button discoButton;
    [SerializeField] private Button drillButton;
    [SerializeField] private Button weaponButton;
    
    
    
    
    // Update is called once per frame
    private void Start()
    {
        
        for (int i = 0; i < shopInterfaceBackground.transform.childCount; i++)
        {
            Transform temp = shopInterfaceBackground.transform.GetChild(i);
            if (temp.gameObject.CompareTag("ShopButton"))
            {
                string addedButton = shopInterfaceBackground.transform.GetChild(i).gameObject.GetComponent<Button>().GetHashCode().ToString();
                buttonDictionary.Add(addedButton, false);
                temp.name = addedButton.ToString();
            }
        }

        foreach (KeyValuePair<string, bool> test in buttonDictionary)
        {
            
            Debug.Log(test.Key + test.Value.ToString());
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
        Debug.Log(shopInterfaceBackground.activeSelf);
    }
    
    private void CloseShopInterface()
    {
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
            buttonDictionary[drillButton.GetHashCode().ToString()] = true;
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
            buttonDictionary[accelerateButton.GetHashCode().ToString()] = true;
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
            buttonDictionary[discoButton.GetHashCode().ToString()] = true;
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
            buttonDictionary[weaponButton.GetHashCode().ToString()] = true;
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
        if (buttonDictionary[drillButton.GetHashCode().ToString()] == false)
        {
            foreach (string hashCode in buttonDictionary.Keys)
            {
                if (!hashCode.Equals(drillButton.GetHashCode().ToString()))
                {
                    FindButton(hashCode).interactable = false;
                    
                }
            }

            
        }
        else
        {
            foreach (string hashCode in buttonDictionary.Keys)
            {
                if (buttonDictionary[hashCode] == false)
                {
                    FindButton(hashCode).interactable = true;
                }
                else
                {
                    
                    FindButton(hashCode).interactable = false;
                }
            }
        }
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
       
        Debug.Log("Run time: " + ts);
    }

    private Button FindButton(string hashCode)
    {
        GameObject temp = GameObject.Find("UI_Shop/ShopCanvas/ShopInterfaceBackground/" + hashCode);
        Button tempButton = temp.GetComponent<Button>();
        return tempButton;
    }

    public void CloseShop()
    {
        CloseShopInterface();
    }
}