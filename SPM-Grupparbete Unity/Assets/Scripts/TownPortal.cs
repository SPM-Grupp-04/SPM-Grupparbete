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
    public static bool isTeleporting;

    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");
        drone = GameObject.Find("Drone");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (!isLoading)
        {
            isTeleporting = true;
            isLoading = true;
            GlobalControl.SaveData();
            SceneManager.LoadScene(5, LoadSceneMode.Additive);
         
            camera.transform.position = new Vector3(1000, camera.transform.position.y, 1000);

            if (playerOne.activeInHierarchy)
            {
                playerOne.transform.position = new Vector3(1000, 3, 1000); // Hamnar på 800/0/550
            }

            if (playerTwo.activeInHierarchy)
            {
                playerTwo.transform.position = new Vector3(1001, 3, 1001);
            }
           
            drone.transform.position =new Vector3(playerOne.transform.position.x,
                drone.transform.position.y, playerOne.transform.position.z);
            
            StartCoroutine(waitUntillActivate());
            isLoading = false;
            
        }
    }

   public static IEnumerator waitUntillActivate()
    {
        yield return new  WaitForSeconds(1);
        isTeleporting = false;

    }
}