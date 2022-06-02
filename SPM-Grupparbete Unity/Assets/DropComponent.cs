using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI explainText;

    // Start is called before the first frame update
    void Start()
    {
        text.text = PlayerStatistics.Instance.componentsCollectedNumber + "/5";

        if (PlayerStatistics.Instance.componentsCollectedNumber > 1)
        {
            explainText.enabled = false;
        }
        else
        {
            explainText.enabled = true;
        }
    }
    
        
            
            
            
        

    

   
}
