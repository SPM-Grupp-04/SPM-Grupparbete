using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Overheat : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private GameObject weapon;
    [SerializeField] Image barColour;
    [SerializeField] PlayerWeapon playerWeaponScript;
	PlayerDrill playerDrillScript;
    private Color greenColor;
    private Color yellowColor;
    private Color redColor;
    private Color orangeColor;

    // Start is called before the first frame update
    void Start()
    {
        playerDrillScript = drill.GetComponent<PlayerDrill>();
         greenColor = new Color(0, 103/255f, 0, 255);
         yellowColor = new Color(103/255f, 103/255f, 0, 255);
         redColor = new Color(103f/255f, 0, 0, 255);
		orangeColor = new Color32(255, 127, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOverheatSlider(playerWeaponScript.WeaponCurrentHeat);
    }

    private void UpdateOverheatSlider(float amount)
    {
        slider.value = amount * 0.01f;

        if (playerWeaponScript.GetComponent<StateMachine>().CurrentState is LaserWeaponCoolingState)
        {
            Debug.Log("Orange");
            barColour.color = orange;
        }
        else if (amount <= playerWeaponScript.OverheatThreshold * 0.5f)
        {
            barColour.color = greenColor;
        }
        else if (amount > playerWeaponScript.OverheatThreshold * 0.5f
            && amount <= playerWeaponScript.OverheatThreshold * 0.8f)
        {
            
            barColour.color = yellowColor;
        } 
        else
        {
            barColour.color = redColor;
        }
    }
}
