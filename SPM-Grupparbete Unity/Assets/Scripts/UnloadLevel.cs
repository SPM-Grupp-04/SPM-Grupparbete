using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UnloadLevel : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject drone;
    private GameObject teleportBackPos;

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

        TownPortal.isTeleporting = true;

        if (playerOne != null)
        {
            playerOne.transform.position = new Vector3(teleportPosition.x + 1
                , 0.58f, teleportPosition.z + 1);
        }

        if (playerTwo != null)
        {
            playerTwo.transform.position = new Vector3(teleportPosition.x 
                , 0.58f, teleportPosition.z );;
        }

        if (drone != null)
        {
            drone.transform.position = new Vector3(teleportPosition.x,
                drone.transform.position.y, teleportPosition.z);
        }
        teleportBackPos.SetActive(false);

        StartCoroutine(TownPortal.waitUntillActivate());

        SceneManager.UnloadSceneAsync(5);
    }
}