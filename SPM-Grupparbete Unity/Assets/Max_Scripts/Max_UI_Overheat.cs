using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Max_UI_Overheat : MonoBehaviour
{

    [SerializeField]private Slider slider;
    [SerializeField] private GameObject drill;
    [SerializeField]Image barColour;
    [SerializeField] Max_Drill max_Drill;

    Color green = new Color(0, 255, 0);
    Color yellow = new Color(255, 255, 0);
    Color red = new Color (255, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        max_Drill = drill.GetComponent<Max_Drill>();

    }

    // Update is called once per frame
    void Update()
    {
        IncreaseOverheatSlider(max_Drill.GetOverheatAmount());
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
