using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectHP : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OreTakeDamage(int amount)
    {
        slider.gameObject.SetActive(true);
        transform.LookAt(Camera.main.transform);
        slider.value -= amount;
        Invoke("HideUI", 2);
        
    }
    private void HideUI()
    {
        slider.gameObject.SetActive(false);
    }
}
