using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialText : MonoBehaviour
{
    
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
       
       canvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        canvas.SetActive(true);
        canvas.transform.rotation = Camera.main.transform.rotation;

        
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
        
    }
}
