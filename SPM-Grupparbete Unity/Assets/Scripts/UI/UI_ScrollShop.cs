using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
 
public class UI_ScrollShop : MonoBehaviour {
 
    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    RectTransform selectedRectTransform;
    GameObject lastSelected;

    void Start() {
        scrollRectTransform = GetComponent<RectTransform>();
        contentPanel = GetComponent<ScrollRect>().content;
    }
 
    void Update() {
        
        
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        // Return if there are none.
        if (selected == null) {
            return;
        }
        // Return if the selected game object is not inside the scroll rect.
        if (selected.transform.parent != contentPanel.transform) {
            return;
        }
        // Return if the selected game object is the same as it was last frame,
        // meaning we haven't moved.
        if (selected == lastSelected) {
            return;
        }
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
        lastSelected = selected;

    }
}