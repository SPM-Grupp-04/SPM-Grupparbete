//author: Simon Canb�ck, sica4801

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class ComponentPickupScript : MonoBehaviour
{
    [SerializeField] private VictoryConditionsScript.Components componentNumber;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private VisualEffect vfx;

    // Start is called before the first frame update
    void Start()
    {
        
        text.enabled = false;
        //checks if the mask contains the value for the component
        if ((PlayerStatistics.Instance.componentsCollectedMask & (int)componentNumber) > 0)
            Destroy(gameObject);

        //yo, did you assign a component value to the object?
        try
        {
            UnityEngine.Assertions.Assert.AreNotEqual(expected: VictoryConditionsScript.Components.UNASSIGNED, actual: componentNumber);
        } catch(UnityEngine.Assertions.AssertionException) {
            Application.Quit();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player") )
            return;
        text.enabled = true;
        canvas.transform.rotation = Camera.main.transform.rotation;
        if (collision.gameObject.GetComponent<PlayerController>().IsUseButtonPressed())
        {
            //the mask now contains the value for the component
            PlayerStatistics.Instance.componentsCollectedMask |= (int)componentNumber;
            GlobalControl.Instance.playerStatistics.componentsCollectedMask =
            PlayerStatistics.Instance.componentsCollectedMask;
            vfx.Play();
            StartCoroutine(Delay());
            Destroy(gameObject);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);

    }
}
