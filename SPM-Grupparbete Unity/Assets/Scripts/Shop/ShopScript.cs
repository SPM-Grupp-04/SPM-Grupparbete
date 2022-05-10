//Main author: Axel Ingelsson Fredler
//Additional programming: Simon Canbäck, sica4801

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField] private GameObject shopInterfaceBackground;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] [Range(1.0f, 10.0f)] private float shopAreaRadius = 5.0f;
    [SerializeField] private Button healButton;
    [SerializeField] private Button accelerateButton;
    [SerializeField] private Button discoButton;
    [SerializeField] private Button drillButton;
    [SerializeField] private Button weaponButton;

    [SerializeField] private int drillLevelCostBlue = 5;
    [SerializeField] private int healCostBlue = 2;
    [SerializeField] private int weaponCostBlue = 5;
    [SerializeField] private int speedCostBlue = 5;
    [SerializeField] private int discoCostBlue = 5;
    private int drillLevelCostRed = 0;

    private Collider[] shopColliders;
    
    [SerializeField] private Button[] buttonsArray;
    private bool shopInterfaceOpened;
    private PlayerState m_PlayerState;

    // Update is called once per frame

    void Awake()
    {
        shopInterfaceBackground.SetActive(false);
        shopInterfaceOpened = false;

        m_PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();

        //buttons = GetComponentsInChildren<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        drillButton.Select();
        CanPlayersHeal();

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;
        if (other.gameObject.GetComponent<PlayerController>().IsUseButtonPressed())
        {

            foreach (Button b in GetComponentsInChildren<Button>(includeInactive: true))
            {
                
                if (b.gameObject.activeSelf)
                {
                    OpenShopInterface(other);
                    break;
                }
            }
        }
        else
        {
            CloseShopInterface(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CloseShopInterface(other);
        
    }

    private void OpenShopInterface(Collider playerCollider)
    {
        shopInterfaceOpened = true;
        
        //playerCollider.gameObject.GetComponent<PlayerController>().SetMovementStatus(false);
        shopInterfaceBackground.SetActive(true);
        Debug.Log(shopInterfaceBackground.activeSelf);
    }

    private void CloseShopInterface(Collider playerCollider)
    {
        shopInterfaceOpened = false;
        //playerCollider.gameObject.GetComponent<PlayerController>().SetMovementStatus(true);
        shopInterfaceBackground.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shopAreaRadius);
    }

    //void OnClick(GameObject go)
    //{
    //}

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
        }
    }

    public void Accelerate(float addedAcceleration)
    {
        if (GlobalControl.Instance.playerStatistics.BlueCrystals >= speedCostBlue)
        {
            accelerateButton.interactable = false;
            m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerOneAcceleration + addedAcceleration);
            m_PlayerState.m_LocalPlayerData.BlueCrystals -= speedCostBlue;

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
}