//Main author: Axel Ingelsson Fredler
//Additional programming: Simon Canbäck, sica4801

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    
    private Dictionary<Button, bool> buttonDictionary;
    [Header("Buttons for shop")]
    [SerializeField] private Button healButton;
    [SerializeField] private Button accelerateButton;
    [SerializeField] private Button discoButton;
    [SerializeField] private Button drillButton;
    [SerializeField] private Button weaponButton;
    
    
    
    
    // Update is called once per frame
    private void Start()
    {
        // if (PlayerStatistics.Instance.buttonDictionary == null)
        // {
        //     Debug.Log("I am empty");
        //     buttonDictionary = new Dictionary<Button, bool>();
        //     
        //     for (int i = 0; i < shopInterfaceBackground.transform.childCount; i++)
        //     {
        //         Transform temp = shopInterfaceBackground.transform.GetChild(i);
        //         if (temp.gameObject.CompareTag("ShopButton"))
        //         {
        //             buttonDictionary.Add(shopInterfaceBackground.transform.GetChild(i).gameObject.GetComponent<Button>(), false);
        //         }
        //     }
        // }
        //
        //
        // foreach (KeyValuePair<Button, bool> test in buttonDictionary)
        // {
        //     Debug.Log("hei");
        //     Debug.Log(test.Key.ToString() + test.Value.ToString());
        // }
        
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
        //PlayerStatistics.Instance.buttonDictionary = buttonDictionary;
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
            //buttonDictionary[drillButton] = true;
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
            //buttonDictionary[accelerateButton] = true;
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
            //buttonDictionary[discoButton] = true;
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
            //buttonDictionary[weaponButton] = true;
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
        if (m_PlayerState.m_LocalPlayerData.drillLevel < 1)
        {
            drillButton.interactable = true;
            healButton.interactable = false;
            discoButton.interactable = false;
            accelerateButton.interactable = false;
            weaponButton.interactable = false;
        }
        else
        {
            drillButton.interactable = false;
            healButton.interactable = true;
            discoButton.interactable = true;
            accelerateButton.interactable = true;
            weaponButton.interactable = true;
        }
        // if (buttonDictionary[drillButton] == false)
        // {
        //     foreach (var button in buttonDictionary.Keys)
        //     {
        //         if (!button.Equals(drillButton))
        //         {
        //             button.interactable = false;
        //         }
        //     }
        // }
        // else
        // {
        //     foreach (var button in buttonDictionary.Keys)
        //     {
        //         if (buttonDictionary[button] == false)
        //         {
        //             button.interactable = true;
        //         }
        //         else
        //         {
        //             button.interactable = false;
        //         }
        //     }
        // }
    }
    
}