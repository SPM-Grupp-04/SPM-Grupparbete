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
    [SerializeField] private GameObject contentGO;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] [Range(1.0f, 30.0f)] private float shopAreaRadius = 5.0f;
    [SerializeField] private bool shopIsFree;
    
    private Collider[] shopColliders;
    private SphereCollider shopCollider;
    private PlayerState m_PlayerState;
    bool pauseButtonPressed;
    private bool GameIsPause;
    private bool doOnce = false;
    
    
    [Header("Costs for misc upgrades")]
    [SerializeField] private int healCostBlue;
    [SerializeField] private int weaponCostBlue;
    [SerializeField] private int speedCostBlue;
    [SerializeField] private int discoCostBlue;
    
    [Header("Cost Drill Upgrades")]
    [SerializeField] private int drillLevelCostBlue;
    [SerializeField] private int drillLevel2CostBlue;
    [SerializeField] private int drillLevel2CostRed;
    
    [Header("Cost Health Upgrades")]
    [SerializeField] private int healthLevel1CostBlue;
    [SerializeField] private int healthLevel2CostBlue;
    [SerializeField] private int healthLevel2CostRed;
    [SerializeField] private int healthLevel3CostBlue;
    [SerializeField] private int healthLevel3CostRed;
    
    [Header("Buttons for shop")]
    [SerializeField] private Button drill1Button;
    [SerializeField] private Button healButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button accelerateButton;
    [SerializeField] private Button healthOneButton;
    [SerializeField] private Button healthTwoButton;
    [SerializeField] private Button healthThreeButton;
    [SerializeField] private Button drill2Button;

    
    private Dictionary<string, bool> buttonDictionary;
    private Stopwatch stopWatch;
    
    private void Start()
    {
        buttonDictionary = PlayerStatistics.Instance.buttonDictionary;
        if (buttonDictionary == null)
        {
            
            buttonDictionary = new Dictionary<string, bool>();
            for (int i = 0; i < contentGO.transform.childCount; i++)
            {
                Transform temp = contentGO.transform.GetChild(i);
                
                if (temp.gameObject.CompareTag("ShopButton"))
                {
                    string addedButton = contentGO.transform.GetChild(i).gameObject.name;
                    buttonDictionary.Add(addedButton, false);
                    temp.name = addedButton;
                }
            }
        }

        // foreach (KeyValuePair<string, bool> test in buttonDictionary)
        // {
        //     
        //     Debug.Log(test.Key + " " + test.Value.ToString());
        // }g

        
        shopCollider = GetComponent<SphereCollider>();
        m_PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        shopCollider.radius = shopAreaRadius;
        shopInterfaceBackground.SetActive(false);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (shopIsFree)
        {
            drillLevelCostBlue = 0;
            healCostBlue = 0;
            weaponCostBlue = 0;
            speedCostBlue = 0;
            discoCostBlue = 0;
            drillLevel2CostBlue = 0;
            drillLevel2CostRed = 0;
            healthLevel1CostBlue = 0;
            healthLevel2CostBlue = 0;
            healthLevel2CostRed = 0;
            
        }
        
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        other.GetComponent<PlayerController>().PlayerCanShop(true);
        UpdateShop(drill1Button);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        if (other.gameObject.GetComponent<PlayerController>().IsMapSwitched())
        {
            OpenShopInterface();
            if (!doOnce)
            {
                drill1Button.Select();
                doOnce = true;
                other.GetComponent<PlayerController>().SetMovementStatus(false);
            }
        }
        else
        {
            CloseShopInterface();
            other.GetComponent<PlayerController>().SetMovementStatus(true);
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerController>().PlayerCanShop(false);
    }

    private void OpenShopInterface()
    {
        shopInterfaceBackground.SetActive(true);
    }
    
    private void CloseShopInterface()
    {
        PlayerStatistics.Instance.buttonDictionary = buttonDictionary;
        doOnce = false;
        shopInterfaceBackground.SetActive(false);

    }
    
    public void Heal()
    {
        
            if (m_PlayerState.m_LocalPlayerData.BlueCrystals >= healCostBlue)
            {
                m_PlayerState.Heal();
                m_PlayerState.m_LocalPlayerData.BlueCrystals -= healCostBlue;
                healButton.Select();
                healButton.interactable = false;
            }
    }

    public void DrillUpgrade(int level)
    {
        switch (level)
        {
            case 1:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevelCostBlue)
                {
                    DrillUpgradeBase(level, drillLevelCostBlue, 0, 0);
                    DisableShopButton(drill1Button);
                    UpdateShop(drill1Button);
                }
                break;
           
            case 2:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevel2CostBlue && GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevel2CostRed)
                {
                    DrillUpgradeBase(level, drillLevel2CostBlue, drillLevel2CostRed, 0);

                    DisableShopButton(drill2Button);
                    UpdateShop(drill2Button);
                }
                break;
        }
    }

    private void DrillUpgradeBase(int level, int blue, int red, int green)
    {
        m_PlayerState.m_LocalPlayerData.drillLevel = level;
        m_PlayerState.m_LocalPlayerData.BlueCrystals -= blue;
        m_PlayerState.m_LocalPlayerData.RedCrystals -= red;
        m_PlayerState.m_LocalPlayerData.GreenCrystals -= green;
        GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
    }

    public void HealthUpgrade(int level)
    {
        switch (level)
        {
            case 1:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel1CostBlue)
                {
                    HealthUpgradeBase(level, healthLevel1CostBlue, 0,0);
                    DisableShopButton(healthOneButton);
                    UpdateShop(healthOneButton);
                }
                break;
            case 2:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel2CostBlue && GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel2CostRed)
                {
                    HealthUpgradeBase(level, healthLevel2CostBlue, healthLevel2CostRed,0);
                    DisableShopButton(healthTwoButton);
                    UpdateShop(healthTwoButton);
                }
                break;
            case 3:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel3CostBlue && GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel3CostRed)
                {
                    HealthUpgradeBase(level, healthLevel3CostBlue, healthLevel3CostRed,0);
                    DisableShopButton(healthThreeButton);
                    UpdateShop(healthThreeButton);
                }
                break;

        }
    }

    private void HealthUpgradeBase(int level, int blue, int red, int green)
    {
        m_PlayerState.m_LocalPlayerData.playerMaxHealth += level * 10;
        m_PlayerState.m_LocalPlayerData.playerOneHealth = m_PlayerState.m_LocalPlayerData.playerMaxHealth;
        m_PlayerState.m_LocalPlayerData.playerTwoHealth = m_PlayerState.m_LocalPlayerData.playerMaxHealth;
        m_PlayerState.m_LocalPlayerData.BlueCrystals -= blue;
        m_PlayerState.m_LocalPlayerData.RedCrystals -= red;
        m_PlayerState.m_LocalPlayerData.GreenCrystals -= green;
        GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
    }

    public void Accelerate(float addedAcceleration)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= speedCostBlue)
        {
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerOneAcceleration + addedAcceleration);
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerTwoAcceleration + addedAcceleration);
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= speedCostBlue;
            DisableShopButton(accelerateButton);
            UpdateShop(accelerateButton);
        }
    }

    // public void Disco(bool isDisco)
    // {
    //     if (GlobalControl.Instance.playerStatistics.BlueCrystals >= discoCostBlue)
    //     {
    //         m_PlayerState.SetDisco(isDisco);
    //         discoButton.interactable = false;
    //         m_PlayerState.m_LocalPlayerData.BlueCrystals -= discoCostBlue;
    //
    //         buttonDictionary[discoButton.name] = true;
    //
    //         discoButton.Select();
    //     }
    // }

    public void WeaponUpgrade(int level)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= weaponCostBlue)
        {
            m_PlayerState.m_LocalPlayerData.weaponLevel = level;
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= weaponCostBlue;
            GameObject.Find("Players/Player1/Drill").GetComponent<PlayerDrill>().SetWeaponLevel();
            GameObject.Find("Players/Player2/Drill").GetComponent<PlayerDrill>().SetWeaponLevel();
            DisableShopButton(weaponButton);
            UpdateShop(weaponButton);
        }
    }
    private void DisableShopButton(Button button)
    {
        button.interactable = false;
        buttonDictionary[button.name] = true;
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

    private void UpdateShop(Button button)
    {
        TestTime(true);

        if (buttonDictionary[drill1Button.name] == false)
        {
            foreach (string buttonName in buttonDictionary.Keys)
            {
                if (!buttonName.Equals(drill1Button.name))
                {
                    FindButton(buttonName).interactable = false;
                    
                }
            }
        }
        else
        {
            foreach (string buttonName in buttonDictionary.Keys)
            {
                Button currentButton = FindButton(buttonName);
                Selectable buttonAbove = currentButton.FindSelectableOnUp();
                if (buttonAbove != null)
                {
                    if (buttonDictionary[buttonAbove.name])
                    {
                        currentButton.interactable = true;
                        Selectable left = currentButton.FindSelectableOnLeft();
                        Selectable right = currentButton.FindSelectableOnRight();

                        if (left != null)
                        {
                            if (buttonDictionary[left.name] == false)
                            {
                                left.interactable = true;
                            }
                            else
                            {
                                left.interactable = false;
                            }

                        }

                        if (right != null)
                        {
                            if (buttonDictionary[right.name] == false)
                            {
                                right.interactable = true;
                            }
                            else
                            {
                                right.interactable = false;
                            }
                        }

                        buttonAbove.interactable = false;
                        if (buttonDictionary[currentButton.name] == true)
                        {
                            currentButton.interactable = false;
                        }

                    }
                    else
                    {
                        currentButton.interactable = false;
                    }
                    
                }
            }
        }
        CanPlayersHeal();
        TestTime(false);
        button.Select();

    }

    private Button FindButton(string buttonName)
    {
        GameObject temp = GameObject.Find("UI_Shop/ShopCanvas/ShopInterfaceBackground/ScrollView/Viewport/Content/" + buttonName);
        Button tempButton = temp.GetComponent<Button>();
        return tempButton;
    }

    private void TestTime(bool value)
    {
        if (value)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }
        else
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Debug.Log("Run time: " + ts);
        }
    }
    
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shopAreaRadius);
    }

    
}