using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RespawnScreen : MonoBehaviour
{
    [SerializeField] private Button _button;
    private void Awake()
    {
        _button.Select();
    }
}
