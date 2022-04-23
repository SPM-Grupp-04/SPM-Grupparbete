using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
   
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

    
}