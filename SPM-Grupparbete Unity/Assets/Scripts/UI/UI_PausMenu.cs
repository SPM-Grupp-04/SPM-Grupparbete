using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    [SerializeField] private GameObject pauseFirstButton;

    
    private void Start()
    { 
       playerInput = GetComponent<PlayerInput>();
       pause = playerInput.actions["Pause"];
       pausePlayerTwo = PlayerInputTwo.actions["Pause"];
    }
    

    public void PauseButtonInput(InputAction.CallbackContext pauseButtonValue)
    {
        if (pauseButtonValue.performed)
        {
            pauseButtonPressed = !pauseButtonPressed;
        }
        if (pauseButtonPressed)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
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
}
