using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EgilCollisionHandler : MonoBehaviour
{
    private EgilHealth eh;
    private float cooldownTime = 2f; // Varför måste den vara i hela sekunder.
    private float timeRemaining;

    private void Start()
    {
        eh = GetComponent<EgilHealth>();
        timeRemaining = cooldownTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Goal"))
        {
            SceneManager.LoadScene(1);
        }
        if (collision.transform.tag.Equals("Currency"))
        {
           collision.gameObject.SendMessage("CollectOre");
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        /*if (collisionInfo.transform.tag.Equals("Enemy") && timeRemaining < 0.0f  )
        {
            Debug.Log("gettingHIt");
            
            eh.DealDamage(1);
        }*/

        /*
        if (timeRemaining < 0.0f)
        {
            timeRemaining = cooldownTime;
        }

        timeRemaining -= Time.deltaTime;
        */

    }

    
}