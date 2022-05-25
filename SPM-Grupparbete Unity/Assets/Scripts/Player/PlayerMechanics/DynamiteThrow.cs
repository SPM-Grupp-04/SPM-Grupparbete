using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Main Author: Axel Ingelsson Fredler

public class DynamiteThrow : MonoBehaviour
{
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private LaunchArcMesh launchArcMesh;
    [SerializeField] private float coolDownTime = 5f;
    [SerializeField] private UI_Cooldowns uiCooldowns;
    private float nextFireTime = 5f;

    [SerializeField] private PlayerDrill playerDrillScript;
    
    private void Update()
    {
        nextFireTime += Time.deltaTime;
        
        if (nextFireTime <= coolDownTime)
        {
            uiCooldowns.GetGrenadeText().text = ((int)coolDownTime - (int)nextFireTime).ToString();
        }

        if (nextFireTime >= coolDownTime)
        {
            uiCooldowns.GetGrenadeText().text = "Kasta";
        }
    }

    public void ThrowDynamite()
    {
        if (nextFireTime >= coolDownTime)
        {
            GameObject thrownDynamite = Instantiate(dynamitePrefab, transform.position, transform.rotation);
            thrownDynamite.GetComponent<Rigidbody>().velocity = launchArcMesh.GetLaunchAngle() * launchArcMesh.GetLaunchVelocity();
            nextFireTime = 0;
            playerDrillScript.IncreaseOverheatAmount(50.0f);
        }
    }
}