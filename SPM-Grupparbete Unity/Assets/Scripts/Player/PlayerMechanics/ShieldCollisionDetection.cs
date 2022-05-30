using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollisionDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().InsideShield = true;
            Debug.Log("Inside Shield: " + other.gameObject.GetComponent<PlayerController>().InsideShield);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().InsideShield = false;
            Debug.Log("Inside Shield: " + other.gameObject.GetComponent<PlayerController>().InsideShield);
        }
    }
}
