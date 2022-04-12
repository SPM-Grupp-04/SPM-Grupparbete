using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class EgilCameraKeepBetweenPlayers : MonoBehaviour
{
    // Kolla de två trasform vi tar in. Räkna ut vart mittpunkten är och flytta dig dit
    [SerializeField] private Transform playerOne; // First object of pair
    [SerializeField] private Transform playerTwo; // Second object of pair
    private Vector3 midpointAtoB = new Vector3(0, 0, 0);

    [FormerlySerializedAs("cvc")] [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Vector3 directionCtoA;
    private Vector3 directionCtoB;
    private const float FOVMULTIPLIER = 6.6f;
    private const int FOVMIN = 80;
    private const int FOVMAX = 100;

    private Vector3 LenghtBetweenPlayers;

    private void Update()
    {
        CalculateMidPointBetweenPlayers();

        LenghtBetweenPlayers = playerOne.position - playerTwo.position;

        SetCameraLensFOV();


        transform.position += midpointAtoB;
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
        cinemachineVirtualCamera.m_Lens.FieldOfView = LenghtBetweenPlayers.magnitude * FOVMULTIPLIER;

        if (cinemachineVirtualCamera.m_Lens.FieldOfView < FOVMIN)
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = FOVMIN;
        }

        if (cinemachineVirtualCamera.m_Lens.FieldOfView > FOVMAX)
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = FOVMAX;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + midpointAtoB, 1);
    }
}