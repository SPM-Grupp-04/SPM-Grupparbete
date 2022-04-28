using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
    

public class UI_Cooldowns : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI grenadeTimer;
    [SerializeField] private TextMeshProUGUI shieldTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TextMeshProUGUI GetShieldText()
    {
        return shieldTimer;
    }

    public TextMeshProUGUI GetGrenadeText()
    {
        return grenadeTimer;
    }
}
