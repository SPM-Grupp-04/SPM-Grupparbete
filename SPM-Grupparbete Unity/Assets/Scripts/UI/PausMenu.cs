using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public PlayerInput playerInput;
    private InputAction pause;

    
    private void Start()
    {
       // playerInput = GetComponent<PlayerInput>();
        pause = playerInput.actions["Pause"];
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.WasPressedThisFrame())
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

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
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
