using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Max_UI_ObjectHP : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OreTakeDamage(int amount)
    {
        slider.enabled = true;
        slider.value -= amount;
    }
    private void HideUI()
    {
        slider.enabled = false;
    }
}
