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

    private CinemachineBrain cmBrain;
    
    public static CameraShake Instance { 
        get 
        {
            return instance;
        }
    }

    private CinemachineVirtualCamera activeCamera;

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
        
        cmBrain = FindObjectOfType<CinemachineBrain>();
    }

    private void Update()
    {
        if (activeCamera != null)
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
    
    public void ShakeCamera(float magnitude, float duration)
    {
        
        FindActiveCamera();
        
        cmBasicMultiChannelPerlin = activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmBasicMultiChannelPerlin.m_AmplitudeGain = magnitude;

        startMagnitude = magnitude;
        totalCameraShakeTime = duration;
        cameraShakeTime = duration;
    }

    private void FindActiveCamera()
    {
        activeCamera = cmBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}
