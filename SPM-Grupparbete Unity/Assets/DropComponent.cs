using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        text.text = PlayerStatistics.Instance.componentsCollectedNumber + "/5";
    }
    
        
            
            
            
        

    

   
}
