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
    
    public GameObject loadingScreen;
    public Slider Slider;
    public Text progressText;
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

        
        StartCoroutine(LoadAsync(5));
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
        teleportBackPos.SetActive(false);

        StartCoroutine(TownPortal.waitUntillActivate());

        //SceneManager.UnloadSceneAsync(5);
    }


    private IEnumerator LoadAsync(int sceneIndex)
    {
      
        loadingScreen.SetActive(true);
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneIndex);
        while (op.isDone == false)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
    
}