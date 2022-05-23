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
    [SerializeField] private RectTransform rectTransform;

    private Collider[] shopColliders;
    private SphereCollider shopCollider;
    private PlayerState m_PlayerState;
    bool pauseButtonPressed;
    private bool GameIsPause;
    private bool doOnce = false;

    [Header("Costs for shop")]
    [SerializeField] private int drillLevelCostBlue;
    [SerializeField] private int healCostBlue;
    [SerializeField] private int weaponCostBlue;
    [SerializeField] private int speedCostBlue;
    [SerializeField] private int discoCostBlue;
    [SerializeField] private int drillLevel2CostRed;
    [SerializeField] private int healthLevel1CostBlue;
    [SerializeField] private int healthLevel2CostBlue;
    [SerializeField] private int healthLevel2CostRed;



    [Header("Buttons for shop")]
    [SerializeField] private Button drill1Button;
    [SerializeField] private Button healButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button accelerateButton;
    [SerializeField] private Button healthOneButton;
    [SerializeField] private Button healthTwoButton;
    [SerializeField] private Button drill2Button;

    private Dictionary<string, bool> buttonDictionary;


    private void Start()
    {
        buttonDictionary = PlayerStatistics.Instance.buttonDictionary;
        //Debug.Log(contentGO.name);

        if (buttonDictionary == null)
        {

            buttonDictionary = new Dictionary<string, bool>();
            for (int i = 0; i < contentGO.transform.childCount; i++)
            {
                Transform temp = contentGO.transform.GetChild(i);

                if (temp.gameObject.CompareTag("ShopButton"))
                {
                    string addedButton = contentGO.transform.GetChild(i).gameObject.name;
                    //Debug.Log(addedButton);
                    buttonDictionary.Add(addedButton, false);
                    temp.name = addedButton;
                }
            }
        }

        foreach (KeyValuePair<string, bool> test in buttonDictionary)
        {

            //Debug.Log(test.Key + " " + test.Value.ToString());
        }


        shopCollider = GetComponent<SphereCollider>();
        m_PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        shopCollider.radius = shopAreaRadius;
        shopInterfaceBackground.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;

        UpdateShop();

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
            }
        }
        else
        {
            CloseShopInterface();
            doOnce = false;
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

    public void DrillUpgrade(PlayerBeamArmamentBase.ArmamentLevel level)
    {
        switch (level)
        {
            case PlayerBeamArmamentBase.ArmamentLevel.LEVEL_1:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevelCostBlue)
                {
                    Debug.Log("I am called");
                    drill1Button.interactable = false;
                    m_PlayerState.m_LocalPlayerData.armamentLevel = level;
                    m_PlayerState.m_LocalPlayerData.BlueCrystals -= drillLevelCostBlue;
                    GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
                    drill1Button.Select();
                    buttonDictionary[drill1Button.name] = true;
                    UpdateShop();
                }
                break;
            
            case PlayerBeamArmamentBase.ArmamentLevel.LEVEL_2:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= 20 && GlobalControl.Instance.playerStatistics.BlueCrystals >= drillLevel2CostRed)
                {
                    Debug.Log("I am called");
                    drill2Button.interactable = false;
                    m_PlayerState.m_LocalPlayerData.armamentLevel = level;
                    m_PlayerState.m_LocalPlayerData.BlueCrystals -= drillLevelCostBlue;
                    m_PlayerState.m_LocalPlayerData.RedCrystals -= drillLevel2CostRed;
                    GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
                    drill2Button.Select();
                    buttonDictionary[drill2Button.name] = true;
                    UpdateShop();
                }
                break;
        }
    }

    public void HealthUpgrade(int level)
    {
        switch (level)
        {
            case 1:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel1CostBlue)
                {
                    Debug.Log("I am called");
                    healthOneButton.interactable = false;
                    m_PlayerState.m_LocalPlayerData.playerMaxHealth += 10;
                    m_PlayerState.m_LocalPlayerData.playerOneHealth = m_PlayerState.m_LocalPlayerData.playerMaxHealth;
                    m_PlayerState.m_LocalPlayerData.playerTwoHealth = m_PlayerState.m_LocalPlayerData.playerMaxHealth;
                    m_PlayerState.m_LocalPlayerData.BlueCrystals -= healthLevel1CostBlue;
                    GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
                    healthOneButton.Select();
                    buttonDictionary[healthOneButton.name] = true;
                    UpdateShop();
                }
                break;
            
            case 2:
                if (GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel2CostBlue && GlobalControl.Instance.playerStatistics.BlueCrystals >= healthLevel2CostRed)
                {
                    Debug.Log("I am called");
                    healthTwoButton.interactable = false;
                    m_PlayerState.m_LocalPlayerData.playerMaxHealth += 20;
                    m_PlayerState.m_LocalPlayerData.playerOneHealth = m_PlayerState.m_LocalPlayerData.playerMaxHealth;
                    m_PlayerState.m_LocalPlayerData.playerTwoHealth = m_PlayerState.m_LocalPlayerData.playerMaxHealth;
                    m_PlayerState.m_LocalPlayerData.BlueCrystals -= healthLevel2CostBlue;
                    m_PlayerState.m_LocalPlayerData.RedCrystals -= healthLevel2CostRed;
                    GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
                    healthTwoButton.Select();
                    buttonDictionary[healthTwoButton.name] = true;
                    UpdateShop();
                }
                break;


        }
    }

    public void Accelerate(float addedAcceleration)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= speedCostBlue)
        {
            accelerateButton.interactable = false;
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerOneAcceleration + addedAcceleration);
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerTwoAcceleration + addedAcceleration);
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= speedCostBlue;

            buttonDictionary[accelerateButton.name] = true;

            accelerateButton.Select();
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
        if ((int)m_PlayerState.m_LocalPlayerData.armamentLevel < 1)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

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
    }

    private Button FindButton(string buttonName)
    {
        Debug.Log(buttonName);
        GameObject temp = GameObject.Find("UI_Shop/ShopCanvas/ShopInterfaceBackground/ScrollView/Viewport/Content/" + buttonName);
        Debug.Log(temp);
        Button tempButton = temp.GetComponent<Button>();


        return tempButton;
    }


}