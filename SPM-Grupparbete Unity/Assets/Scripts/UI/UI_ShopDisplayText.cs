using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopDisplayText : MonoBehaviour
{
    [SerializeField] private Text textHeader;
    [SerializeField] private Text textCost;
    [SerializeField] private Text textBody;
    [SerializeField] private GameObject[] buttonsArray;

    private string header = "";

    private string cost = "";

    private string body = "";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText(GameObject gameObject)
    {
        int i = FindIndex(gameObject);
        switch (i)
        {
            case 0:
                header = "Drill Upgrade";
                cost = "5";
                body = "This makes drill go brrrr";
                break;
            case 1:
                header = "Heal";
                cost = "5";
                body = "This heals the player, many HP";
                break;
            case 2:
                header = "Weapon Upgrade";
                cost = "10";
                body = "This makes laser go far";
                break;
            case 3:
                header = "Speed Upgrade";
                cost = "10";
                body = "This makes players gor wroom";
                break;
            case 4:
                header = "Disco Upgrade";
                cost = "50";
                body = "Boots and cats, Boots and cats, Boots and cats, Boots and cats";
                break;
            
        }
        
        
        textHeader.fontSize = 70;
        textHeader.text += header + "\n";
        textCost.fontSize = 40;
        textCost.text += cost + "\n";
        textBody.fontSize = 25;
        textBody.text += body;
    }

    public void RemoveDisplayText()
    {
        textHeader.text = "";
        textCost.text = "";
        textBody.text = "";
    }

    private int FindIndex(GameObject gameObject)
    {
        for (int i = 0; i < buttonsArray.Length; i++)
        {
            if (buttonsArray.GetValue(i).Equals(gameObject))
            {
                return i;
            }
        }

        return -1;
    }


}
