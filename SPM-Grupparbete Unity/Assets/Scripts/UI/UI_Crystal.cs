using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crystal : MonoBehaviour
{

    private PlayerStatistics m_LocalPlayerDataTest = PlayerStatistics.Instance;


    [SerializeField]
    private Text title;
    



    public void Update()
    {

        title.text = m_LocalPlayerDataTest.Crystals.ToString();
    }
}
