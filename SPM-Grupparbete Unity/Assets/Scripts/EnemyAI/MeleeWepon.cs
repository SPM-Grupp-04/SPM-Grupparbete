using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;

public class MeleeWepon : MonoBehaviour
{
    private float cooldownTime = 0.5f;
    private float timeRemaining;

    private void Start()
    {
        timeRemaining = cooldownTime;
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("HIt player");
        
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

    /*
    {
        Debug.Log("HIt player");
        
        if (timeRemaining < 0.0f)
        {
            var damageEvent = new DealDamageEventInfo(collision.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);
        }

        if (timeRemaining < 0.0f)
        {
            timeRemaining = cooldownTime;
        }

        timeRemaining -= Time.deltaTime;
    }
*/
}
