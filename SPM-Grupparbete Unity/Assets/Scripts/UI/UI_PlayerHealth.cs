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

    Color green = new Color(0, 255, 0);
    Color yellow = new Color(255, 255, 0);
    Color red = new Color(255, 0, 0);

    private void Update()
    {
        ChangeHealthPlayerOne(m_LocalPlayerDataTest.playerOneHealth);
        ChangeHealthPlayerTwo(m_LocalPlayerDataTest.playerTwoHealth);

    }

    private void ChangeHealthPlayerOne(float amount)
    {
        sliderPlayerOne.value = amount;
        if (m_LocalPlayerDataTest.playerOneHealth >= 4)
        {
            barColourPlayerOne.color = green;
        }
        else if (m_LocalPlayerDataTest.playerOneHealth > 1 && m_LocalPlayerDataTest.playerOneHealth <= 3)
        {
            barColourPlayerOne.color = yellow;
        }
        else
        {
            barColourPlayerOne.color = red;
        }
   
    }
    private void ChangeHealthPlayerTwo(float amount)
    {
        sliderPlayerTwo.value = amount;
        if (m_LocalPlayerDataTest.playerTwoHealth >= 4)
        {
            barColourPlayerTwo.color = green;
        }
        else if (m_LocalPlayerDataTest.playerTwoHealth > 1 && m_LocalPlayerDataTest.playerTwoHealth <= 3)
        {
            barColourPlayerTwo.color = yellow;
        }
        else
        {
            barColourPlayerTwo.color = red;
        }
    }
}
