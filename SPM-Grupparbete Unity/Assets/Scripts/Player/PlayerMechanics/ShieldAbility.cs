using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ShieldAbility : MonoBehaviour
{

    [SerializeField] private float timerToDestroyShield = 2f;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] GameObject player;
    [SerializeField] private float cooldownToNextUse = 5f;
    [SerializeField] private UI_Cooldowns uiCooldowns;

    private GameObject shieldGO;
    private float timer = 0;
    
    private float nextShieldTime = 5f;

    private static bool canUseShield;

    private bool shieldButtonPressed;

    private void Awake()
    {
        canUseShield = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
        }
        if(timer == 0 && shieldGO != null)
        {
            DestroyShield();
        }
        
        if (shieldButtonPressed)
        {
            ActivateShield();
        }

        nextShieldTime += Time.deltaTime;

        if (nextShieldTime <= cooldownToNextUse) {}

        {
            uiCooldowns.GetShieldText().text = ((int) cooldownToNextUse - (int) nextShieldTime).ToString();
        }
        
        if (nextShieldTime >= cooldownToNextUse)
        {
            uiCooldowns.GetShieldText().text = "Sköld";
        }
        
    }

    public void ShieldButtonInput(InputAction.CallbackContext shieldButtonValue)
    {
        shieldButtonPressed = shieldButtonValue.performed;
    }

    public void ActivateShield()
    {
        if (canUseShield && nextShieldTime >= cooldownToNextUse)
        {
            timer = timerToDestroyShield;
            shieldGO = Instantiate(shieldPrefab, player.transform.position, player.transform.rotation);
            canUseShield = false;
            nextShieldTime = 0;
        }
    }

    private void DestroyShield()
    {
         Destroy(shieldGO);
         canUseShield = true;
    }
}
