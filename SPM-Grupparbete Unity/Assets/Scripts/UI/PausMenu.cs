using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PausMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public PlayerInput playerInput;
    public PlayerInput PlayerInputTwo;
    private InputAction pause;
    private InputAction pausePlayerTwo;

    private bool pauseButtonPressed;
    
    [Header("MenuButtonFirstSelection")]
    [SerializeField] private GameObject pauseFirstButton;

    
    private void Start()
    {
      // playerInput = GetComponent<PlayerInput>();
      // pause = playerInput.actions["Pause"];
      // pausePlayerTwo = PlayerInputTwo.actions["Pause"];
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseButtonPressed)
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                
                Pause();
            }
            
        }
    }

    public void PauseButtonInput(InputAction.CallbackContext pauseButtonValue)
    {
        if (pauseButtonValue.performed)
        {
            pauseButtonPressed = !pauseButtonPressed;
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
