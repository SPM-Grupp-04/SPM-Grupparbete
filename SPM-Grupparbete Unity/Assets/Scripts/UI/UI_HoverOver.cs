
using UnityEngine;
using UnityEngine.EventSystems;


public class UI_HoverOver : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject displayWindow;
    private GameObject target;
    private void Awake()
    {
        displayWindow = GameObject.Find("TextWindow");
        target = GameObject.Find("Selected");
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        displayWindow.GetComponent<UI_ShopDisplayText>().DisplayText(this.gameObject);
        target.transform.position = this.transform.position;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        displayWindow.GetComponent<UI_ShopDisplayText>().RemoveDisplayText();
        
        
    }
}
