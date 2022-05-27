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

    [Header("E1 = Blue, E2 = Red, E3 = Green")]
    [Header("Costs for misc upgrades")]
    [SerializeField] private int[] healCost = new int[3];
    [SerializeField] private int[] weaponCost = new int[3];
    [SerializeField] private int[] speedCost = new int[3];
    [SerializeField] private int[] discoCost = new int[3];
    
    [Header("Cost Drill Upgrades")]
    [SerializeField] private int[] drillLevel1Cost = new int[3];
    [SerializeField] private int[] drillLevel2Cost = new int[3];
    [SerializeField] private int[] drillLevel3Cost = new int[3];

    
    [Header("Cost Health Upgrades")]
    [SerializeField] private int[] healthLevel1Cost = new int[3];
    [SerializeField] private int[] healthLevel2Cost = new int[3];
    [SerializeField] private int[] healthLevel3Cost = new int[3];

    
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
    private List<Collider> playersInShop = new List<Collider>();
    private Stopwatch stopWatch;

    private GameObject playerOneDrill;
    private GameObject playerTwoDrill;
    
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

        playerOneDrill = GameObject.Find("Players/Player1/Drill");
        playerTwoDrill = GameObject.Find("Players/Player2/Drill");
        
        shopCollider = GetComponent<SphereCollider>();
        m_PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        shopCollider.radius = shopAreaRadius;
        shopInterfaceBackground.SetActive(false);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (shopIsFree)
        {
            healCost = new int[3];
            weaponCost = new int[3];
            speedCost = new int[3];
            discoCost = new int[3];

            drillLevel1Cost = new int[3];
            drillLevel2Cost = new int[3];
            drillLevel3Cost = new int[3];
            
            healthLevel1Cost = new int[3];
            healthLevel2Cost = new int[3];
            healthLevel3Cost = new int[3];
            
        }
        
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        other.GetComponent<PlayerController>().PlayerCanShop(true);
        UpdateShop(drill1Button);
        //playersInShop.Add(other);
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
        
            if (m_PlayerState.m_LocalPlayerData.BlueCrystals >= healCost[0])
            {
                m_PlayerState.Heal();
                m_PlayerState.m_LocalPlayerData.BlueCrystals -= healCost[0];
                healButton.interactable = false;
                healButton.Select();
            }
    }

    public void DrillUpgrade(int level)
    {
        switch (level)
        {
            case 1:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevel1Cost[0] 
                    && GlobalControl.Instance.playerStatistics.RedCrystals >= drillLevel1Cost[1]
                    && GlobalControl.Instance.playerStatistics.GreenCrystals >= drillLevel1Cost[2])
                {
                    DrillUpgradeBase(level, drillLevel1Cost[0], drillLevel1Cost[1], drillLevel1Cost[2]);
                    DisableShopButton(drill1Button);
                    UpdateShop(drill1Button);
                }
                break;
           
            case 2:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevel2Cost[0] 
                    && GlobalControl.Instance.playerStatistics.RedCrystals >= drillLevel2Cost[1] 
                    && GlobalControl.Instance.playerStatistics.GreenCrystals >= drillLevel2Cost[2])
                {
                    DrillUpgradeBase(level, drillLevel2Cost[0], drillLevel2Cost[1], drillLevel2Cost[2]);
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
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel1Cost[0]
                    && GlobalControl.Instance.playerStatistics.RedCrystals >= healthLevel1Cost[1]
                    && GlobalControl.Instance.playerStatistics.GreenCrystals >= healthLevel1Cost[2])
                {
                    HealthUpgradeBase(level, healthLevel1Cost[0], healthLevel1Cost[1],healthLevel1Cost[2]);
                    DisableShopButton(healthOneButton);
                    UpdateShop(healthOneButton);
                }
                break;
            case 2:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel2Cost[0] 
                    && GlobalControl.Instance.playerStatistics.RedCrystals >= healthLevel2Cost[1]
                    && GlobalControl.Instance.playerStatistics.GreenCrystals >= healthLevel2Cost[2])
                {
                    HealthUpgradeBase(level, healthLevel2Cost[0], healthLevel2Cost[1], healthLevel2Cost[2]);
                    DisableShopButton(healthTwoButton);
                    UpdateShop(healthTwoButton);
                }
                break;
            case 3:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel3Cost[0] 
                    && GlobalControl.Instance.playerStatistics.RedCrystals >= healthLevel3Cost[1]
                    && GlobalControl.Instance.playerStatistics.GreenCrystals >= healthLevel3Cost[2])
                {
                    HealthUpgradeBase(level, healthLevel1Cost[0], healthLevel1Cost[1],healthLevel3Cost[2]);
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
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= speedCost[0]
            && GlobalControl.Instance.playerStatistics.RedCrystals >= speedCost[1]
            && GlobalControl.Instance.playerStatistics.GreenCrystals >= speedCost[2])
        {
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerOneAcceleration + addedAcceleration);
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerTwoAcceleration + addedAcceleration);
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= speedCost[0];
            m_PlayerState.m_LocalPlayerData.RedCrystals -= speedCost[1];
            m_PlayerState.m_LocalPlayerData.GreenCrystals -= speedCost[2];
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
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= weaponCost[0])
        {
            m_PlayerState.m_LocalPlayerData.weaponLevel = level;
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= weaponCost[0];
            playerOneDrill.GetComponent<PlayerDrill>().SetWeaponLevel();
            playerTwoDrill.GetComponent<PlayerDrill>().SetWeaponLevel();
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
//            Debug.Log("Run time: " + ts);
        }
    }
    
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shopAreaRadius);
    }

    
}