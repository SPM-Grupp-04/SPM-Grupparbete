using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EgilCollisionHandeler : MonoBehaviour
{
    private EgilHealth eh;
    private float cooldownTime = 2f; // Varför måste den vara i hela sekunder.
    private float timeRemaning;
    private void Start()
    {
        eh = GetComponent<EgilHealth>();
        timeRemaning = cooldownTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Goal"))
        {
            SceneManager.LoadScene(1);
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.tag.Equals("Enemy") && timeRemaning < 0  )
        {
            Debug.Log("gettingHIt");
            
            eh.Takedamage();
           
        }

        if (timeRemaning < 0)
        {
            timeRemaning = cooldownTime;
        }
        timeRemaning -= Time.deltaTime;

    }

    
}