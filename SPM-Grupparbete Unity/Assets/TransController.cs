using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransController : MonoBehaviour
{
    private CanvasGroup image;
    [SerializeField] private float speed = 2;
    private void Start()
    {
        image = GetComponent<CanvasGroup>();
    }

    private int count = 0;
    private void FixedUpdate()
    {
        if (shouldITransistion)
        {
            image.alpha -= speed * count  * Time.deltaTime;
            count += 2 ;
        }

        if (image.alpha <= 0)
        {
            count = 0;
            image.alpha = 1;
            shouldITransistion = false;
            gameObject.SetActive(false);
        }
    }

    private bool shouldITransistion;
    private void OnEnable()
    {

        shouldITransistion = true;

    }

    
}
