using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
 
public class UI_ScrollShop : MonoBehaviour {
 
    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    RectTransform selectedRectTransform;
 
    void Start() {
        scrollRectTransform = GetComponent<RectTransform>();
        contentPanel = GetComponent<ScrollRect>().content;
    }
 
    void Update() {
        
        GameObject selected = EventSystem.current.currentSelectedGameObject;
 
        selectedRectTransform = selected.GetComponent<RectTransform>();
        
        float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        float scrollViewMinY = contentPanel.anchoredPosition.y;
        float scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;
 
        if (selectedPositionY > scrollViewMaxY) {
            float newY = selectedPositionY - scrollRectTransform.rect.height;
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
        }
        else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < scrollViewMinY) {
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y));
        }

    }
}