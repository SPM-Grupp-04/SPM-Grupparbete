using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopDisplayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHeader;
    [SerializeField] private TextMeshProUGUI textCostBlue;
    [SerializeField] private TextMeshProUGUI textCostRed;
    [SerializeField] private TextMeshProUGUI textCostGreen;
    [SerializeField] private TextMeshProUGUI textBody;
    [SerializeField] private GameObject shop;
    private ShopScript shopScript;
    List<int[]> shopCostsArray;
    
    private string header = "";

    private int costBlue;
    private int costRed;
    private int costGreen;
    
    private string body = "";
    
    // Start is called before the first frame update
    void Start()
    {
        shopScript = shop.GetComponent<ShopScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /* Index for shop costs in list.
     * healCost,0
     * weaponCost, 1
     * speedCost, 2
     * discoCost, 3
     * drillLevel1Cost, 4
     * drillLevel2Cost, 5
     * drillLevel3Cost, 6
     * healthLevel1Cost, 7
     * healthLevel2Cost, 8
     * healthLevel3Cost, 9
     * droneCost, 10
     */

    public void DisplayText(GameObject gameObject)
    {
        shopCostsArray = shopScript.GetShopCostArrays();
        switch (gameObject.name)
        {
            
            case "HealButton":
                header = "Heal";
                SetPriceShopText(0);
                body = "This heals the player and revives dead robots";
                break;
            case "WeaponButton":
                header = "Weapon Upgrade";
                SetPriceShopText(1);
                body = "Weapon reaches even further";
                break;
            case "SpeedButton":
                header = "Speed Upgrade";
                SetPriceShopText(2);
                body = "This makes the players go faster";
                break;
            case "DrillButton":
                header = "Drill One";
                SetPriceShopText(4);
                body = "Upgrades the drill so you can get through blue wall";
                break;
            case "DrillButton2":
                header = "Drill Two";
                SetPriceShopText(5);
                body = "Upgrades the drill so you can mine the red crystals and walls";
                break;
            case "DrillButton3":
                header = "Drill Three";
                SetPriceShopText(6);
                body = "With this upgrade you will be able to mine the green crystals and walls";
                break;
            case "Health1Button":
                header = "Health One";
                SetPriceShopText(7);
                body = "Player gets 10 extra HP";
                break;
            case "Health2Button":
                header = "Health Two";
                SetPriceShopText(8);
                body = "Player gets 20 extra HP";
                break;
            case "Health3Button":
                header = "Health Three";
                SetPriceShopText(9);
                body = "Player gets 30 extra HP";
                break;
            case "DroneButton":
                header = "Drone";
                SetPriceShopText(10);
                body = "Players get a drone that mines and shoots";
                break;
        }
        
        
        textHeader.fontSize = 40;
        textHeader.text += header + "\n";
        
        textCostBlue.fontSize = 35;
        textCostBlue.text += costBlue + "\n";
        textCostRed.fontSize = 35;
        textCostRed.text += costRed + "\n";
        textCostGreen.fontSize = 35;
        textCostGreen.text += costGreen + "\n";
        
        textBody.fontSize = 35;
        textBody.text += body;
    }

    private void SetPriceShopText(int i)
    {
        costBlue = shopCostsArray[i][0];
        costRed = shopCostsArray[i][1];
        costGreen = shopCostsArray[i][2];

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
