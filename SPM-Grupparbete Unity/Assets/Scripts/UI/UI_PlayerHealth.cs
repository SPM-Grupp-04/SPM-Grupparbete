using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider sliderPlayerOne;
    [SerializeField] private Slider sliderPlayerTwo;
    
    [SerializeField] Image barColourPlayerOne;
    [SerializeField] Image barColourPlayerTwo;

    private PlayerStatistics m_LocalPlayerDataTest = PlayerStatistics.Instance;

    Color green = new Color(0, 103/255f, 0);
    Color yellow = new Color(103/255f, 103/255f, 0);
    Color red = new Color(103/255f, 0, 0);
    

    private void Update()
    {
        ChangeHealthPlayerOne(m_LocalPlayerDataTest.playerOneHealth);
        ChangeHealthPlayerTwo(m_LocalPlayerDataTest.playerTwoHealth);

    }
    

    private void ChangeHealthPlayerOne(float amount)
    {
        sliderPlayerOne.maxValue = PlayerStatistics.Instance.playerMaxHealth;

        sliderPlayerOne.value = amount;
        
   
    }
    private void ChangeHealthPlayerTwo(float amount)
    {
        sliderPlayerTwo.maxValue = PlayerStatistics.Instance.playerMaxHealth;

        sliderPlayerTwo.value = amount;
     
    }
}
