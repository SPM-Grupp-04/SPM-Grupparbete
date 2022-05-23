using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI infoText;
    

    [SerializeField] private Canvas canvas; 
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        infoText.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.enabled = true;
            infoText.enabled = true;
            

            if (other.gameObject.GetComponent<PlayerController>().IsUseButtonPressed())
            {
                text.text = GlobalControl.Instance.playerStatistics.componentsCollectedMask.ToString() + "/5";
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        text.enabled = false;
        infoText.enabled = false;
    }
}
