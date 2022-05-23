using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crystal : MonoBehaviour
{

    private PlayerStatistics m_LocalPlayerDataTest = PlayerStatistics.Instance;


    [SerializeField]
    private Text BlueCrystals;
    [SerializeField]
    private Text RedCrystals;




    public void Update()
    {

        BlueCrystals.text = m_LocalPlayerDataTest.BlueCrystals.ToString();
        RedCrystals.text = m_LocalPlayerDataTest.RedCrystals.ToString();

    }
}
