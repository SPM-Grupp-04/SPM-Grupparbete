using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnloadLevel : MonoBehaviour
{
    private GameObject players;
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject drone;
    private GameObject droneOne;
    private GameObject teleportBackPos;
    private GameObject Trans;
   
    private void Start()
    {
        players = GameObject.Find("Players");
        playerOne = players.transform.Find("Player1").gameObject;
        playerTwo = players.transform.Find("Player2").gameObject;
        drone = GameObject.Find("Drone");
        droneOne = GameObject.Find("Drone (1)");
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
        
        if (droneOne != null)
        {
            droneOne.transform.position = new Vector3(teleportPosition.x,
                droneOne.transform.position.y, teleportPosition.z);
        }
       
        
        StartCoroutine(TownPortal.waitUntillActivate());

        
    }


   
    
}