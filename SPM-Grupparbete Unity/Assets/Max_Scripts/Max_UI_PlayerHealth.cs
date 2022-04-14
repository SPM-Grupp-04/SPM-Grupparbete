using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Max_UI_PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider sliderPlayerOne;
    [SerializeField] private Slider sliderPlayerTwo;
    
    [SerializeField] Image barColourPlayerOne;
    [SerializeField] Image barColourPlayerTwo;

    private EgilPlayerStatistics localEgilPlayerDataTest = EgilPlayerStatistics.Instance;

    Color green = new Color(0, 255, 0);
    Color yellow = new Color(255, 255, 0);
    Color red = new Color(255, 0, 0);

    private void Update()
    {
        ChangeHealthPlayerOne(localEgilPlayerDataTest.PlayerOneHealth);
        ChangeHealthPlayerTwo(localEgilPlayerDataTest.PlayerTwoHealth);

    }

    private void ChangeHealthPlayerOne(int amount)
    {
        sliderPlayerOne.value = amount;
        if (localEgilPlayerDataTest.PlayerOneHealth >= 4)
        {
            barColourPlayerOne.color = green;
        }
        else if (localEgilPlayerDataTest.PlayerOneHealth > 1 && localEgilPlayerDataTest.PlayerOneHealth <= 3)
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
        if (localEgilPlayerDataTest.PlayerTwoHealth >= 4)
        {
            barColourPlayerTwo.color = green;
        }
        else if (localEgilPlayerDataTest.PlayerTwoHealth > 1 && localEgilPlayerDataTest.PlayerTwoHealth <= 3)
        {
            barColourPlayerTwo.color = yellow;
        }
        else
        {
            barColourPlayerTwo.color = red;
        }
    }
}
