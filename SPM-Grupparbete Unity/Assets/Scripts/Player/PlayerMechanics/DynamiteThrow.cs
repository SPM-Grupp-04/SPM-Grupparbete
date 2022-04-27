using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamiteThrow : MonoBehaviour
{
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private LaunchArcMesh launchArcMesh;
    [SerializeField] private float coolDownTime = 5f;
    private float nextFireTime;

    private void Update()
    {
        //Debug.Log(transform.forward);
    }

    public void ThrowDynamite(InputAction.CallbackContext value)
    {
        if (Time.time > nextFireTime)
        {
            if (value.performed)
            {
                GameObject thrownDynamite = Instantiate(dynamitePrefab, transform.position, transform.rotation);
                thrownDynamite.GetComponent<Rigidbody>().velocity = launchArcMesh.GetLaunchAngle() * launchArcMesh.GetLaunchVelocity();
                nextFireTime = Time.time + coolDownTime;
            }
        }
        
    }
}
