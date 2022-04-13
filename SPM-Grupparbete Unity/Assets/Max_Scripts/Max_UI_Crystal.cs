using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Max_UI_Crystal : MonoBehaviour
{

    private EgilPlayerStatistics localEgilPlayerDataTest = EgilPlayerStatistics.Instance;


    [SerializeField]
    private Text title;
    



    public void Update()
    {

        title.text = localEgilPlayerDataTest.Crystals.ToString();
    }
}
