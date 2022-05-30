using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopDisplayText : MonoBehaviour
{
    [SerializeField] private Text textHeader;
    [SerializeField] private Text textCostBlue;
    [SerializeField] private Text textCostRed;
    [SerializeField] private Text textCostGreen;
    [SerializeField] private Text textBody;
    [SerializeField] private GameObject shop;
    
    private string header = "";

    private int costBlue;
    private int costRed;
    private int costGreen;


    private string body = "";
    
    private List<int[]> arrayList;
    /*
     healCost, 0 
     weaponCost, 1 
     speedCost, 2
     discoCost, 3
     drillLevel1Cost, 4
     drillLevel2Cost, 5
     drillLevel3Cost, 6
     healthLevel1Cost, 7
     healthLevel2Cost, 8
     healthLevel3Cost 9
     */
    
    public void DisplayText(GameObject gameObject)
    {
        arrayList = shop.GetComponent<ShopScript>().GetShopArrays();
        
        switch (gameObject.name)
        {
            
            case "HealButton":
                header = "Heal";
                SetCosts(0);
                body = "This heals the player, many HP";
                break;
            case "WeaponButton":
                header = "Weapon Upgrade";
                SetCosts(1);
                body = "This makes laser go far";
                break;
            case "SpeedButton":
                header = "Speed Upgrade";
                SetCosts(2);
                body = "This makes players gor wroom";
                break;
            case "DrillButton":
                header = "Drill Upgrade";
                SetCosts(4);
                body = "This makes drill go brrrr";
                break;
            case "DrillButton2":
                header = "Drill 2";
                SetCosts(5);
                body = "This makes drill go even more BBBRRRRRRRR";
                break;
            case "DrillButton3":
                header = "Drill 3";
                SetCosts(6);
                body = "This makes drill go even more BBBRRRRRRRR";
                break;
            case "Health1Button":
                header = "Health 1";
                SetCosts(7);
                body = "Player gets 10 extra HP";
                break;
            case "Health2Button":
                header = "Health 2";
                SetCosts(8);
                body = "Player gets 20 extra HP";
                break;
            case "Health3Button":
                header = "Health 3";
                SetCosts(9);
                body = "Player gets 30 extra HP";
                break;
        }
        
        
        textHeader.fontSize = 70;
        textHeader.text += header + "\n";
        textCostBlue.fontSize = 40;
        textCostBlue.text += costBlue + "\n";
        textCostRed.fontSize = 40;
        textCostRed.text += costRed + "\n";
        textCostGreen.fontSize = 40;
        textCostGreen.text += costGreen + "\n";
        textBody.fontSize = 25;
        textBody.text += body;
    }
    
    private void SetCosts(int value)
    {
        costBlue = arrayList[value][0];
        costRed = arrayList[value][1];
        costGreen = arrayList[value][2];
    }

    public void RemoveDisplayText()
    {
        textHeader.text = "";
        textCostBlue.text = "";
        textCostRed.text = "";
        textCostGreen.text = "";
        textBody.text = "";
    }
    


}
