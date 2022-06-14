using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Component : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI component;

    // Update is called once per frame
    void FixedUpdate()
    {
        component.text = PlayerStatistics.Instance.componentsCollectedNumber + "/5";
    }
}
