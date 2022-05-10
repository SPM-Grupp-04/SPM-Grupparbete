using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
        text.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        canvas.transform.rotation = Camera.main.transform.rotation;

        text.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        text.gameObject.SetActive(false);
    }
}
