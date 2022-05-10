using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text text;

    [SerializeField]
    private string header;

    [SerializeField] private string cost;

    [SerializeField] private string body;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontSize = 40;
        text.text += header + "\n";
        text.fontSize = 30;
        text.text += cost + "\n";
        text.fontSize = 25;
        text.text += body;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.text = "";
    }
}
