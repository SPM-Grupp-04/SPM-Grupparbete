using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;

    [SerializeField] private GameObject canvasToSetActive;
    
    /*
    [Header("Zonen man vill spawna i Default är 2 som är hub")]
    [SerializeField] private int zoneToSpawnIn = 2;*/

    
    [Header("If testing set to false")]
    public bool useRespawn;
    
    private void Update()
    {
        if (useRespawn && playerOne.activeInHierarchy == false && playerTwo.activeInHierarchy == false)
        {
            
            // Tar dig till Hubben.
            canvasToSetActive.SetActive(true);
            //SceneManager.LoadScene(zoneToSpawnIn);
        }
    }
}