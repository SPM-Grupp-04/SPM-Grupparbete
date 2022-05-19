using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;

public class MeleeWepon : MonoBehaviour
{
    private float cooldownTime = 0.5f;
    public float timeRemaining;

    private void Start()
    {
        timeRemaining = cooldownTime;
    }

   
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("SHould trigger");
        if (timeRemaining < 0.0f)
        {
           
            var damageEvent = new DealDamageEventInfo(other.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);
        }

        if (timeRemaining < 0.0f)
        {
            timeRemaining = cooldownTime;
        }

        timeRemaining -= Time.deltaTime;
    }

   
}
