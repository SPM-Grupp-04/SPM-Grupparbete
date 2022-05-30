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
}
