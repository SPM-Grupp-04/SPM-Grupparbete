using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Overheat : MonoBehaviour
{

    [SerializeField]private Slider slider;
    [SerializeField] private GameObject drill;
    [SerializeField]Image barColour;
    [SerializeField] PlayerDrill playerDrillScript;

    Color green = new Color(0, 255, 0);
    Color yellow = new Color(255, 255, 0);
    Color red = new Color (255, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerDrillScript = drill.GetComponent<PlayerDrill>();

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
            barColour.color = green;
        }
        else if (amount > 50 && amount <= 80) 
        {
            barColour.color = yellow;
        } 
        else
        {
            barColour.color = red;
        }
       
    }

    
}
