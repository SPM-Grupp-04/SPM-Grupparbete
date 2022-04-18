using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class EgilCameraHandler : MonoBehaviour
{
    // Kolla de två trasform vi tar in. Räkna ut vart mittpunkten är och flytta dig dit
    [SerializeField] private Transform playerOne; // First object of pair
    [SerializeField] private Transform playerTwo; // Second object of pair
    private Vector3 midpointAtoB = new Vector3(0, 0, 0);
    [SerializeField] private CinemachineVirtualCamera virtualCameraTwoPlayers;

    [SerializeField] private CinemachineVirtualCamera virtualCameraPlayerOne;

    [SerializeField] private CinemachineVirtualCamera virtualCameraPlayerTwo;


    private Vector3 directionCtoA;
    private Vector3 directionCtoB;
    private const float FOVMULTIPLIER = 6.6f;
    private const int FOVMIN = 80;
    private const int FOVMAX = 100;

    private const int ActivateCamera = 10;
    private const int DeactivateCamera = 0;

    private Vector3 LenghtBetweenPlayers;


    private void Start()
    {
        virtualCameraPlayerOne.Priority = 0;
        virtualCameraPlayerTwo.Priority = 0;
    }


    private void Update()
    {
        CheckWhatCameraShouldHavePriority();

        CalculateMidPointBetweenPlayers();

        LenghtBetweenPlayers = playerOne.position - playerTwo.position;

        SetCameraLensFOV();

        transform.position += midpointAtoB;
    }

    private void CheckWhatCameraShouldHavePriority()
    {


        if (playerOne.gameObject.activeInHierarchy && playerTwo.gameObject.activeInHierarchy)
        {
            virtualCameraPlayerOne.Priority = DeactivateCamera;
            virtualCameraPlayerTwo.Priority = DeactivateCamera;
            virtualCameraTwoPlayers.Priority = ActivateCamera;
        }
        
        if (virtualCameraTwoPlayers.Priority > 0 && (playerOne.gameObject.activeInHierarchy == false ||
                                                     playerTwo.gameObject.activeInHierarchy == false))
        {
            virtualCameraTwoPlayers.Priority = DeactivateCamera;
        }

        if (playerOne.gameObject.activeInHierarchy == false)
        {
            virtualCameraTwoPlayers.Priority = DeactivateCamera;
            virtualCameraPlayerTwo.Priority = ActivateCamera;
        }

        if (playerTwo.gameObject.activeInHierarchy == false)
        {
            virtualCameraTwoPlayers.Priority = DeactivateCamera;
            virtualCameraPlayerOne.Priority = ActivateCamera;
        }
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