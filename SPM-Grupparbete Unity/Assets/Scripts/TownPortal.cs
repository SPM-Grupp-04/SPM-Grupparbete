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
    [SerializeField] new private GameObject camera;
    private bool isLoading;
    public static bool isTeleporting;

    private readonly static float activationDelay = 1.0f;

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
            playerOne.transform.position = new Vector3(1000, 3, 1000); // Hamnar på 800/0/550
            playerTwo.transform.position = new Vector3(1001, 3, 1001);
            drone.transform.position = new Vector3(playerOne.transform.position.x, drone.transform.position.y, playerOne.transform.position.z);
            StartCoroutine(WaitUntilActivate());
            isLoading = false;
        }
    }

    public static IEnumerator WaitUntilActivate()
    {
        yield return new WaitForSeconds(activationDelay);
        isTeleporting = false;
    }
}