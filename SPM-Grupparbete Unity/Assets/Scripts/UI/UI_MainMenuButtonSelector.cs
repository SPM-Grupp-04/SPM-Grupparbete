using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenuButtonSelector : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Slider settingsMenuSlider;
    [SerializeField] private Button showCrontrollerMenuButton;
    
    [SerializeField] private Button showCustomizeMenuButton;

    [SerializeField] private Slider playerOneCustomSlider;
    [SerializeField] private Slider playerTwoCustomSlider;


    public void MainMenu()
    {
        mainMenuButton.Select();
    }

    public void SettingsMenu()
    {
        settingsMenuSlider.Select();
    }

    public void ShowControllerMenu()
    {
        showCrontrollerMenuButton.Select();
    }

    public void ShowCustomizeMenu()
    {
        showCustomizeMenuButton.Select();
    }

    public void ShowCustomizePlayerOneMenu()
    {
        playerOneCustomSlider.Select();
    }
    public void ShowCustomizePlayerTwoMenu()
    {
        playerTwoCustomSlider.Select();   
    }
}
