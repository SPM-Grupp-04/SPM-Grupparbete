using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownPortal : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject drone;
    [SerializeField]   private GameObject camera;
    private bool isLoading;

    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");
        drone = GameObject.Find("Drone");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLoading)
        {
            isLoading = true;
            GlobalControl.Instance.playerStatistics.PosX = playerOne.transform.position.x;
            GlobalControl.Instance.playerStatistics.PosY = playerOne.transform.position.y;
            GlobalControl.Instance.playerStatistics.PosZ = playerOne.transform.position.z;
            GlobalControl.SaveData();
            SceneManager.LoadScene(5, LoadSceneMode.Additive);
         
            camera.transform.position = new Vector3(1000, camera.transform.position.y, 1000);
            playerOne.transform.position = new Vector3(1000, 3, 1000); // Hamnar på 800/0/550
            playerTwo.transform.position = new Vector3(1001, 3, 1001);
            drone.transform.position = playerOne.transform.position;
            
        }
    }
}