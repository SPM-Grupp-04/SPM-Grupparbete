using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main author: Axel Ingelsson Fredler

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject shopInterfaceBackground;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] [Range(1.0f, 10.0f)] private float shopAreaRadius = 5.0f;
    private Collider[] shopColliders;
    private bool shopInterfaceOpened;

    // Update is called once per frame

    void Awake()
    {
        shopInterfaceBackground.SetActive(false);
        shopInterfaceOpened = false;
    }

    void Update()
    {
        DetectPlayerAndOpenShop();
    }

    private void DetectPlayerAndOpenShop()
    {
        shopColliders = Physics.OverlapSphere(transform.position, shopAreaRadius, playerLayerMask);
        foreach (Collider collider in shopColliders)
        {
            if (collider.gameObject.GetComponent<PlayerController>().IsUseInputPressed())
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

    private void OpenShopInterface(Collider playerCollider)
    {
        shopInterfaceOpened = true;
        playerCollider.gameObject.GetComponent<PlayerController>().SetMovementStatus(false);
        shopInterfaceBackground.SetActive(true);
    }

    private void CloseShopInterface(Collider playerCollider)
    {
        shopInterfaceOpened = false;
        playerCollider.gameObject.GetComponent<PlayerController>().SetMovementStatus(true);
        shopInterfaceBackground.SetActive(false);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shopAreaRadius);
    }
}
