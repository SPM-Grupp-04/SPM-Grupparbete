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
        ChangeHealthPlayerOne(m_LocalPlayerDataTest.PlayerOneHealth);
        ChangeHealthPlayerTwo(m_LocalPlayerDataTest.PlayerTwoHealth);

    }

    private void ChangeHealthPlayerOne(int amount)
    {
        sliderPlayerOne.value = amount;
        if (m_LocalPlayerDataTest.PlayerOneHealth >= 4)
        {
            barColourPlayerOne.color = green;
        }
        else if (m_LocalPlayerDataTest.PlayerOneHealth > 1 && m_LocalPlayerDataTest.PlayerOneHealth <= 3)
        {
            barColourPlayerOne.color = yellow;
        }
        else
        {
            barColourPlayerOne.color = red;
        }
   
    }
    private void ChangeHealthPlayerTwo(int amount)
    {
        sliderPlayerTwo.value = amount;
        if (m_LocalPlayerDataTest.PlayerTwoHealth >= 4)
        {
            barColourPlayerTwo.color = green;
        }
        else if (m_LocalPlayerDataTest.PlayerTwoHealth > 1 && m_LocalPlayerDataTest.PlayerTwoHealth <= 3)
        {
            barColourPlayerTwo.color = yellow;
        }
        else
        {
            barColourPlayerTwo.color = red;
        }
    }
}
