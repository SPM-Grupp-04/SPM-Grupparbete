using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnloadLevel : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject drone;
    private GameObject teleportBackPos;
    private GameObject Trans;
   
    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");
        drone = GameObject.Find("Drone");
        teleportBackPos = GameObject.Find("TownPortal");
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        var teleportPosition = teleportBackPos.transform.position;
        teleportBackPos.SetActive(false);
        TownPortal.IsTeleporting = true;

        SceneManager.UnloadSceneAsync(5);
       
        if (playerOne != null)
        {
            playerOne.transform.position = new Vector3(teleportPosition.x + 1
                , 0.8f, teleportPosition.z + 1);
        }

        if (playerTwo != null)
        {
            playerTwo.transform.position = new Vector3(teleportPosition.x 
                , 0.8f, teleportPosition.z );;
        }

        if (drone != null)
        {
            drone.transform.position = new Vector3(teleportPosition.x,
                drone.transform.position.y, teleportPosition.z);
        }
       
        
        StartCoroutine(TownPortal.waitUntillActivate());

        
    }


   
    
}