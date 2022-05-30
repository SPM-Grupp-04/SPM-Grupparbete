using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HideButtons : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    public void SetButtonState(bool value)
    {
        pauseMenu.GetComponent<UI_PausMenu>().SetButtonState(value);
        
    }



}
