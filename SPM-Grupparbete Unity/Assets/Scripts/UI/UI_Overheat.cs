using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Overheat : MonoBehaviour
{

    [SerializeField]private Slider slider;
    [SerializeField] private GameObject weapon;
    [SerializeField]Image barColour;
    [SerializeField] PlayerWeapon playerWeaponScript;

    Color green = new Color(0, 255, 0);
    Color yellow = new Color(255, 255, 0);
    Color red = new Color (255, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerWeaponScript = weapon.GetComponent<PlayerWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOverheatSlider(playerWeaponScript.WeaponCurrentHeat);
    }

    private void UpdateOverheatSlider(float amount)
    {
        slider.value = amount * 0.01f;

        if (amount <= playerWeaponScript.OverheatThreshold * 0.5f)
        {
            barColour.color = green;
        }
        else if (amount > playerWeaponScript.OverheatThreshold * 0.5f 
            && amount <= playerWeaponScript.OverheatThreshold * 0.8f) 
        {
            barColour.color = yellow;
        } 
        else
        {
            barColour.color = red;
        }
       
    }

    
}
