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

    [SerializeField] private int drillLevelCostBlue = 5;
    private int drillLevelCostRed = 0;

    private Collider[] shopColliders;

    //private Button[] buttons;
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

    void Update()
    {
        //DetectPlayerAndOpenShop();
    }

    private void DetectPlayerAndOpenShop()
    {
        shopColliders = Physics.OverlapSphere(transform.position, shopAreaRadius, playerLayerMask);

        foreach (Collider collider in shopColliders)
        {
            if (collider.gameObject.GetComponent<PlayerController>().IsUseButtonPressed())
            {
                if (!shopInterfaceOpened)
                {
                    OpenShopInterface(collider);
                }
                else
                {
                    CloseShopInterface(collider);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Utility.LayerMaskExtensions.IsInLayerMask(other.gameObject, playerLayerMask))
            return;

        foreach (Button b in GetComponentsInChildren<Button>(includeInactive: true))
        {
            if (b.gameObject.activeSelf)
            {
                OpenShopInterface(other);
                break;
            }
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

    public void Heal(int healAmount)
    {
        if (m_PlayerState.m_LocalPlayerData.BlueCrystals > 2)
        {
            m_PlayerState.Heal(healAmount);
        }

        //  healButton.gameObject.SetActive(false);
        //  CloseShopInterface(null);
    }

    public void drillUpgrade(int level)
    {
      
 
        if (GlobalControl.Instance.playerStatistics.BlueCrystals > drillLevelCostBlue)
        {
            m_PlayerState.m_LocalPlayerData.drillLevel = level;

            GlobalControl.Instance.playerStatistics = PlayerStatistics.Instance;
        }
    }

    public void Accelerate(float addedAcceleration)
    {
        m_PlayerState.SetAcceleration(PlayerStatistics.Instance.playerOneAcceleration + addedAcceleration);

        accelerateButton.gameObject.SetActive(false);
        CloseShopInterface(null);
    }

    public void Disco(bool isDisco)
    {
        Debug.Log("DISCO!");
        m_PlayerState.SetDisco(isDisco);

        discoButton.gameObject.SetActive(false);
        CloseShopInterface(null);
    }
}