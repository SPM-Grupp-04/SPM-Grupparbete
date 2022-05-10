using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_HoverOver : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject displayWindow;

    private void Awake()
    {
        displayWindow = GameObject.Find("TextWindow");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnSelect(BaseEventData eventData)
    {
        displayWindow.GetComponent<UI_ShopDisplayText>().DisplayText(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        displayWindow.GetComponent<UI_ShopDisplayText>().RemoveDisplayText();
    }
}
