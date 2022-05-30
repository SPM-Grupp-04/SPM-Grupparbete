using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialText : MonoBehaviour
{
    
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;
    private PlayerController PlayerControllerPlayerOne;
    private PlayerController PlayerControllerPlayerTwo;

    private void Awake()
    {
        PlayerControllerPlayerOne = playerOne.GetComponent<PlayerController>();
        PlayerControllerPlayerTwo = playerTwo.GetComponent<PlayerController>();

       canvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerControllerPlayerOne.IsShopOpen() || PlayerControllerPlayerOne.IsPauseMenuOpen()
            || PlayerControllerPlayerTwo.IsShopOpen() || PlayerControllerPlayerTwo.IsPauseMenuOpen())
        {
            canvas.SetActive(false);
            return;
        }
        canvas.SetActive(true);
        canvas.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
        
    }
}
