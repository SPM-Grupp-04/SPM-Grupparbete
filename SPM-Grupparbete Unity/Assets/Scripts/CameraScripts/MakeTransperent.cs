using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.CameraScripts;

public class MakeTransperent : MonoBehaviour
{
     private List<IamInTheWay> currentlyInTheWay;
       private List<IamInTheWay> alreadyInTheWay;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerTwo;
    private Transform camera;

    private void Awake()
    {
        currentlyInTheWay = new List<IamInTheWay>();
        alreadyInTheWay = new List<IamInTheWay>();
        camera = this.gameObject.transform;
    }

    private void Update()
    {
        GetAllObjectsInTheWay();
        MakeObjectsSolid();
        MakeObjectTransperant();
    }

    private void GetAllObjectsInTheWay()
    {
        currentlyInTheWay.Clear();
        
        Vector3 cameraPos = camera.position;
        float cameraPlayerDistance = Vector3.Magnitude(cameraPos - player.position);
        float cameraPlayerTwoDistance = Vector3.Magnitude(cameraPos - playerTwo.position);
        
       

        Ray rayPlayerOne = new Ray(cameraPos, player.position - cameraPos  );
        Ray rayPlayerTwo = new Ray(cameraPos,  playerTwo.position - cameraPos );

        
        
        RaycastHit[] hitOne = Physics.RaycastAll(rayPlayerOne, cameraPlayerDistance);
        RaycastHit[] hitTwo = Physics.RaycastAll(rayPlayerTwo, cameraPlayerTwoDistance);


        if (hitOne.Length == 0)
        {
            Debug.Log("HIt nothing");
        }
        else if(hitOne.Length >= 1 )
        {
            for (int i = 0; i < hitOne.Length; i++)
            {
                Debug.Log(hitOne[i].collider.gameObject.name);
            }
        }

   
        Debug.DrawRay(cameraPos, player.position- cameraPos, Color.black);
        Debug.DrawRay(cameraPos, playerTwo.position- cameraPos, Color.cyan);

        foreach (var hit in hitOne)
        {
            if (hit.collider.gameObject.TryGetComponent(out IamInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }

        foreach (var hit in hitTwo)
        {
            if (hit.collider.gameObject.TryGetComponent(out IamInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
    }

    private void MakeObjectTransperant()
    {

        foreach (var VARIABLE in currentlyInTheWay)
        {
            if (!alreadyInTheWay.Contains(VARIABLE))
            {
                VARIABLE.ShowTransperant();
                alreadyInTheWay.Add(VARIABLE);
            }

        }
        
    }

    private void MakeObjectsSolid()
    {
        
        foreach (var VARIABLE in alreadyInTheWay)
        {
            if (!currentlyInTheWay.Contains(VARIABLE))
            {
                VARIABLE.ShowSolidBody();
                alreadyInTheWay.Remove(VARIABLE);
            }

        }
        
    }
}