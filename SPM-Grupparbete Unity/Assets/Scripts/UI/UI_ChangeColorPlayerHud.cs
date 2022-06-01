using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChangeColorPlayerHud : MonoBehaviour
{
    [SerializeField] private Image playerOneHud;
    [SerializeField] private Image playerTwoHud;
    [SerializeField] private Image playerOneFillOH;
    [SerializeField] private Image playerTwoFillOH;
    [SerializeField] private Image playerOneFillHP;
    [SerializeField] private Image playerTwoFillHP;
    
    // Start is called before the first frame update
    void Start()
    {
        Color color = playerOneHud.color;
        if (PlayerStatistics.Instance.playerOneColor.Length == 0)
        {
            color.r = 0.15f;
            color.g = 0.75f;
            color.b = 0.75f;
        }
        else
        {
            
            color.r = PlayerStatistics.Instance.playerOneColor[0];
            color.g = PlayerStatistics.Instance.playerOneColor[1];
            color.b = PlayerStatistics.Instance.playerOneColor[2];
        }
        playerOneHud.color = color;
        playerOneFillOH.color = color;
        playerOneFillHP.color = color;
        
        if (PlayerStatistics.Instance.playerTwoColor.Length == 0)
        {
            color.r = 0.75f;
            color.g = 0.75f;
            color.b = 0.15f;
        }
        else
        {
            
            color.r = PlayerStatistics.Instance.playerTwoColor[0];
            color.g = PlayerStatistics.Instance.playerTwoColor[1];
            color.b = PlayerStatistics.Instance.playerTwoColor[2];
        }
        playerTwoHud.color = color;
        playerTwoFillOH.color = color;
        playerTwoFillHP.color = color;

    }

    
}
