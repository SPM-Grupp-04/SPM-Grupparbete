using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraHandler : MonoBehaviour
{
    // Kolla de två trasform vi tar in. Räkna ut vart mittpunkten är och flytta dig dit
    [SerializeField] private Transform playerOne; // First object of pair
    [SerializeField] private Transform playerTwo; // Second object of pair


    private Vector3 midpointAtoB = new Vector3(0, 0, 0);
    [SerializeField] private CinemachineVirtualCamera virtualCameraTwoPlayers;

    [SerializeField] private CinemachineVirtualCamera virtualCameraPlayerOne;

    [SerializeField] private CinemachineVirtualCamera virtualCameraPlayerTwo;

    private static CameraHandler instance;
    
    private Camera mainCamera;
    
    public static CameraHandler Instance
    {
        get { return instance; }
    }

    private Vector3 directionCtoA;
    private Vector3 directionCtoB;
    private const float FOVMULTIPLIER = 6.6f;
    private const int FOVMIN = 80;
    private const int FOVMAX = 100;

    private Vector3 LenghtBetweenPlayers;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }


    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        virtualCameraPlayerOne.gameObject.SetActive(false);
        virtualCameraPlayerTwo.gameObject.SetActive(false);
    }


    private void Update()
    {
        CheckWhatCameraShouldHavePriority();

        CalculateMidPointBetweenPlayers();

        LenghtBetweenPlayers = playerOne.position - playerTwo.position;

        SetCameraLensFOV();

        transform.position += new Vector3(midpointAtoB.x, 0, midpointAtoB.z);

    }

    private void CheckWhatCameraShouldHavePriority()
    {
        if (playerOne.gameObject.activeInHierarchy && playerTwo.gameObject.activeInHierarchy)
        {
            virtualCameraPlayerOne.gameObject.SetActive(false);
            virtualCameraPlayerTwo.gameObject.SetActive(false);
            virtualCameraTwoPlayers.gameObject.SetActive(true);
        }

        if ((playerOne.gameObject.activeInHierarchy == false || playerTwo.gameObject.activeInHierarchy == false))
        {
            virtualCameraTwoPlayers.gameObject.SetActive(false);
        }

        if (playerOne.gameObject.activeInHierarchy == false)
        {
            virtualCameraPlayerTwo.gameObject.SetActive(true);
        }

        if (playerTwo.gameObject.activeInHierarchy == false)
        {
            virtualCameraPlayerOne.gameObject.SetActive(true);
        }
    }
    
    private void RestrictMovement()
    {
        Vector3 cameraView = mainCamera.WorldToViewportPoint(transform.position);
        cameraView.x = Mathf.Clamp01(cameraView.x);
        cameraView.y = Mathf.Clamp01(cameraView.y);

        bool isOutSide = false;

        if (cameraView.x == 0f || cameraView.x == 1)
        {
            isOutSide = true;
            //  Debug.Log("Outside X ");
        }

        if (cameraView.y == 0f || cameraView.y == 1)
        {
            isOutSide = true;
            //  Debug.Log("Outside Y");
        }

        Vector3 playerPosInWorldPoint = mainCamera.ViewportToWorldPoint(cameraView);
        if (isOutSide)
        {
            //    Debug.Log("PlayerPosInWorld " + playerPosInWorldPoint);
            Debug.Log("IS OUTSIDE");
        }

        transform.position = new Vector3(playerPosInWorldPoint.x, transform.position.y, playerPosInWorldPoint.z);
    }

    private void CalculateMidPointBetweenPlayers()
    {
        directionCtoA = playerOne.position - transform.position;
        directionCtoB = playerTwo.position - transform.position;

        midpointAtoB = new Vector3((directionCtoA.x + directionCtoB.x) / 2.0f,
            (directionCtoA.y + directionCtoB.y) / 2.0f,
            (directionCtoA.z + directionCtoB.z) / 2.0f);
    }

    private void SetCameraLensFOV()
    {
        virtualCameraTwoPlayers.m_Lens.FieldOfView = LenghtBetweenPlayers.magnitude * FOVMULTIPLIER;

        if (virtualCameraTwoPlayers.m_Lens.FieldOfView < FOVMIN)
        {
            virtualCameraTwoPlayers.m_Lens.FieldOfView = FOVMIN;
        }

        if (virtualCameraTwoPlayers.m_Lens.FieldOfView > FOVMAX)
        {
            virtualCameraTwoPlayers.m_Lens.FieldOfView = FOVMAX;
        }
    }
}