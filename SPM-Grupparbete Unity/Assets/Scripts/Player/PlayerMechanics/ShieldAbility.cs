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
    

    private PlayerInput playerInput;

    private InputAction shieldAction;
    

    private GameObject shieldGO;
    private float timer = 0;
    
    private float nextShieldTime;

    private static bool canUseShield;

    private void Awake()
    {
        
        playerInput = GetComponent<PlayerInput>();
        shieldAction = playerInput.actions["Shield"];
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

        
        
        if (shieldAction.IsPressed())
        {
            ActivateShield();
        }
        
       


    }

    public void ActivateShield()
    {
        if (canUseShield && Time.time > nextShieldTime)
        {
            timer = timerToDestroyShield;
            shieldGO = Instantiate(shieldPrefab, player.transform.position, player.transform.rotation);
            canUseShield = false;
            Debug.Log("Activate " + nextShieldTime);
        }

    }

    private void DestroyShield()
    {
         Destroy(shieldGO);
         nextShieldTime = Time.time + cooldownToNextUse;
         canUseShield = true;
         Debug.Log("Destroy " + cooldownToNextUse + " " + nextShieldTime);
    }
}
