using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Overheat : MonoBehaviour
{

    [SerializeField]private Slider slider;
    [SerializeField] private GameObject drill;
    [SerializeField]Image barColour;
    PlayerDrill playerDrillScript;

    private Color greenColor;
    private Color yellowColor;
    private Color redColor;

    // Start is called before the first frame update
    void Start()
    {
        playerDrillScript = drill.GetComponent<PlayerDrill>();
         greenColor = new Color(0, 103/255f, 0, 255);
         yellowColor = new Color(103/255f, 103/255f, 0, 255);
         redColor = new Color(103f/255f, 0, 0, 255);

    }

    // Update is called once per frame
    void Update()
    {
        IncreaseOverheatSlider(playerDrillScript.GetOverheatAmount());
    }

    private void IncreaseOverheatSlider(float amount)
    {
        slider.value = amount * 0.01f;
        if (amount <= 50)
        {
            barColour.color = greenColor;
        }
        else if (amount > 50 && amount <= 80) 
        {
            
            barColour.color = yellowColor;
        } 
        else
        {
            barColour.color = redColor;
        }
    }
}
