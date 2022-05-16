using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ShieldAbility : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float upTimeShield = 5f;
    [SerializeField] private float coolDown = 15f;
    [SerializeField] private UI_Cooldowns uiCooldowns;
    
    private static float cooldownToNextUse;
    private GameObject shieldGO;
    private float destroyShieldTimer;
    private static bool canUseShield;

    


    private bool shieldButtonPressed;

    private void Awake()
    {
        coolDown += upTimeShield;
        canUseShield = true;

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
            uiCooldowns.GetShieldText().text = ((int)cooldownToNextUse - (int)Time.time).ToString();
        }
        else
        { 
            uiCooldowns.GetShieldText().text = "Sköld";
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
            Debug.Log("SMIL");
            destroyShieldTimer = upTimeShield;
            shieldGO = Instantiate(shieldPrefab, player.transform.position, player.transform.rotation);
            canUseShield = false;
            cooldownToNextUse = Time.time + coolDown;
        }
    }

    private void DestroyShield()
    {   
         
         Destroy(shieldGO);
         
    }
}
