using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PausMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    [SerializeField] private GameObject pauseMenuUI;
    
    
    public PlayerInput playerInput;
    public PlayerInput PlayerInputTwo;
    private InputAction pause;
    private InputAction pausePlayerTwo;
    
    
    private bool pauseButtonPressed;
    
    [Header("MenuButtonFirstSelection")]
    [SerializeField] private Button pauseFirstButton;

    [Header("Buttons in menu")] [SerializeField]
    private GameObject[] buttonsInMenu;

    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
       //pause = playerInput.actions["Pause"];
       pausePlayerTwo = PlayerInputTwo.actions["Pause"];
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        SetButtonState(true);
        Time.timeScale = 0f;
        pauseFirstButton.Select();
        GameIsPause = true;

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;

    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void SetButtonState(bool value)
    {
        foreach (GameObject button in buttonsInMenu)
        {
            button.SetActive(value);
        }

      
    }
    
}
