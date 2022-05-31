using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CustomizePlayer : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer materialPlayerOne;
    [SerializeField] SkinnedMeshRenderer materialPlayerTwo;

    [SerializeField] private Slider red;
    [SerializeField] private Slider blue;
    [SerializeField] private Slider green;
    // Update is called once per frame
    public void EditMaterial(string player)
    {
        if (player.Equals("PlayerOne"))
        {
            Color color = materialPlayerOne.material.color;
            color.r = red.value;
            color.g = green.value;
            color.b = blue.value;
            materialPlayerOne.material.color = color;
            PlayerStatistics.Instance.playerOneColor = new[] { red.value, green.value, blue.value };
        }
        else
        {
            Color color = materialPlayerTwo.material.color;
            color.r = red.value;
            color.g = green.value;
            color.b = blue.value;
            materialPlayerTwo.material.color = color;
            PlayerStatistics.Instance.playerTwoColor = new[] { red.value, green.value, blue.value };
        }
    }
}
