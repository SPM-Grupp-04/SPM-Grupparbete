using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadLevel : MonoBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;
    private GameObject teleportBackPos;
    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");
        teleportBackPos = GameObject.Find("TowPortal");
    }

    private void OnTriggerEnter(Collider other)
    {
        
        playerOne.transform.position = new Vector3(teleportBackPos.transform.position.x -300,
            0.58f, teleportBackPos.transform.position.z - 500 );
       
        playerTwo.transform.position =  new Vector3(teleportBackPos.transform.position.x -300,
            0.58f, teleportBackPos.transform.position.z - 500 );
        SceneManager.UnloadSceneAsync(5);

    }
}
