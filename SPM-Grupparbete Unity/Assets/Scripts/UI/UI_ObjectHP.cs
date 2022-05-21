using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_ObjectHP : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject gameObjectHP;
    [SerializeField] private float hideSliderTimer = 2.0f;
    

    private void Start()
    {
        MinableOre ore = gameObjectHP.gameObject.GetComponent<MinableOre>();
        DestroyableWall wall = gameObjectHP.gameObject.GetComponent<DestroyableWall>();
        
        slider.gameObject.SetActive(false);

        if (ore != null)
        {
            slider.maxValue = ore.GetOreMaterialHP();
            slider.value = ore.GetOreMaterialHP();
        }

        if ( wall != null)
        {
            slider.maxValue = wall.GetWallHP();
            slider.value = wall.GetWallHP();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ObjectTakeDamage(int amount)
    {
        CancelInvoke("HideUI");
        slider.gameObject.SetActive(true);

        transform.rotation = Camera.main.transform.rotation;

        slider.value -= amount;
        Invoke("HideUI", hideSliderTimer);
    }

    private void HideUI()
    {
        slider.gameObject.SetActive(false);
    }
}
