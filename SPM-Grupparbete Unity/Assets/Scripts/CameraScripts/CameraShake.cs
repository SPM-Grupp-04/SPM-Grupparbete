using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cinemachine;

//Main Author: Axel Ingelsson Fredler

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    
    public static CameraShake Instance { 
        get 
        {
            return instance;
        }
    }

    [SerializeField] private CinemachineVirtualCamera cmVirtualCamera;

    private float cameraShakeTime;
    private float totalCameraShakeTime;

    private float startMagnitude;

    private CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        cmBasicMultiChannelPerlin =
            cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float magnitude, float duration)
    {
        cmBasicMultiChannelPerlin.m_AmplitudeGain = magnitude;

        startMagnitude = magnitude;
        totalCameraShakeTime = duration;
        cameraShakeTime = duration;
    }

    private void Update()
    {
        if (cmVirtualCamera != null)
        {
            if (cameraShakeTime > 0.0f)
            {
                cameraShakeTime -= Time.deltaTime;
                if (cameraShakeTime <= 0.0f)
                {
                    cmBasicMultiChannelPerlin.m_AmplitudeGain = 
                        Mathf.Lerp(startMagnitude, 0.0f, 1.0f - (cameraShakeTime / totalCameraShakeTime));
                }
            }
        }
    }
}
