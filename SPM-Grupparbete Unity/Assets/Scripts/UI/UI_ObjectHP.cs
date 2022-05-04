using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectHP : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject minableOre;
    

    private void Start()
    {
        slider.gameObject.SetActive(false);
        slider.maxValue = minableOre.GetComponent<MinableOre>().GetOreMaterialHP();
        slider.value = minableOre.GetComponent<MinableOre>().GetOreMaterialHP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OreTakeDamage(int amount)
    {
        slider.gameObject.SetActive(true);

        transform.rotation = Camera.main.transform.rotation;

        slider.value -= amount;
        Invoke("HideUI", 2);
        
    }
    private void HideUI()
    {
        slider.gameObject.SetActive(false);
    }
}
