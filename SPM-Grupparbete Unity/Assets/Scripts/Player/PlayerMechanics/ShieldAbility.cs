using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldAbility : MonoBehaviour
{
    private static bool canUseShield;

    [SerializeField] private static float coolDownTimerStart = 2f;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] GameObject player;

    private PlayerInput playerInput;

    private InputAction shieldAction;
    


    private GameObject shieldGO;
    private float timer = 0;

    private void Awake()
    {
        canUseShield = true;
        playerInput = GetComponent<PlayerInput>();
        shieldAction = playerInput.actions["Shield"];

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
            DestoryShield();
        }
        if (shieldAction.IsPressed())
        {     
           
            ActivateShield();
        }


    }

    public void ActivateShield()
    {
        if (canUseShield)
        {
            canUseShield = false;
            timer = coolDownTimerStart;
            shieldGO = Instantiate(shieldPrefab, player.transform.position, player.transform.rotation);
        }
    }

    private void DestoryShield()
    {
         Destroy(shieldGO);
        canUseShield = true;
    }
}
