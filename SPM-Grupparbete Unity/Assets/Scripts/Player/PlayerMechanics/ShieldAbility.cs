using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShieldAbility : MonoBehaviour
{
    private PlayerStatistics playerStatistics = PlayerStatistics.Instance;

    [SerializeField] GameObject player;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float upTimeShield = 5f;
    [SerializeField] private UI_Cooldowns uiCooldowns;

    private Image uiIconBW;
    
    private float coolDown = 15f;
    private static float cooldownToNextUse;
    private GameObject shieldGO;
    private float destroyShieldTimer;
    private static bool canUseShield;
    private float shieldCooldownModifer;
    
    private bool shieldButtonPressed;

    private void Start()
    {
        uiIconBW = GameObject.Find("UI/Cooldowns/ShieldTestBW").GetComponent<Image>();
        shieldCooldownModifer = playerStatistics.shieldCooldownModifer;
        coolDown += upTimeShield;
        canUseShield = true;
        uiIconBW.fillAmount = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (destroyShieldTimer > 0)
        {
            destroyShieldTimer -= Time.deltaTime;
            if (destroyShieldTimer < 0)
            {
                destroyShieldTimer = 0;
            }
        }
        
        if(destroyShieldTimer == 0 && shieldGO != null)
        {
            DestroyShield();
        }
        
        if (shieldButtonPressed)
        {
            ActivateShield();
        }

        if (cooldownToNextUse >= Time.time)
        {

            uiIconBW.fillAmount -= 1 / (coolDown * 2) * Time.deltaTime;
            uiCooldowns.GetShieldText().text = ((int)cooldownToNextUse - (int)Time.time).ToString();

        }
        else
        { 
            //uiCooldowns.GetShieldText().text = "Sköld";
            uiIconBW.fillAmount = 0;
            uiCooldowns.GetShieldText().text = "";
            canUseShield = true;
        }
    }

    public void ShieldButtonInput(InputAction.CallbackContext shieldButtonValue)
    {
        shieldButtonPressed = shieldButtonValue.performed;
    }

    public void ActivateShield()
    {

        if (canUseShield &&  Time.time >= cooldownToNextUse)
        {
            destroyShieldTimer = upTimeShield;
            shieldGO = Instantiate(shieldPrefab, player.transform.position, player.transform.rotation);
            canUseShield = false;
            uiIconBW.fillAmount = 1;
            cooldownToNextUse = Time.time + coolDown;
        }
    }

    private void DestroyShield()
    {   
         
         Destroy(shieldGO);
         
    }
}
