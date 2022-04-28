using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter))]
public class LaunchArcMesh : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction enterDynamiteThrowModeInputAction;
    private InputAction increaseTrajectoryArcInputAction;
    private Mesh trajectoryMesh;
    [SerializeField] private float meshWidth;
    
    [SerializeField] private float velocity = 0.0f;
    [SerializeField] private float angle;
    [SerializeField] private float trajectoryArcIncreaseSpeed = 0.01f;
    [SerializeField] private int lineSegments = 10;

    [SerializeField] private LayerMask groundLayerMask;

    private float gravity;
    private float radianAngle;

    public float GetLaunchVelocity()
    {
        return velocity;
    }

    public Vector3 GetLaunchAngle()
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
        Vector3 localDirection = rotation * transform.forward + transform.up;
        return localDirection.normalized;
    }

    private void OnValidate()
    {
        if (trajectoryMesh != null && Application.isPlaying)
        {
            RenderThrowTrajectoryMesh(CalculateThrowTrajectoryArray());
        }
    }

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        enterDynamiteThrowModeInputAction = playerInput.actions["EnterDynamiteThrowMode"];
        increaseTrajectoryArcInputAction = playerInput.actions["TrajectoryIncrease"];
        trajectoryMesh = GetComponent<MeshFilter>().mesh;
        gravity = Mathf.Abs(Physics.gravity.y);
    }
    
    private void Update()
    {
        if (enterDynamiteThrowModeInputAction.IsPressed())
        {
            TrajectoryPrediction();
        }
        else
        {
            DisableTrajectoryPrediction();
        }
    }

    private void TrajectoryPrediction()
    {
        velocity += increaseTrajectoryArcInputAction.ReadValue<float>() * trajectoryArcIncreaseSpeed;
        angle += increaseTrajectoryArcInputAction.ReadValue<float>() * 0.005f;
        RenderThrowTrajectoryMesh(CalculateThrowTrajectoryArray());
    }

    private void DisableTrajectoryPrediction()
    {
        trajectoryMesh.Clear();
    }

    private void RenderThrowTrajectoryMesh(Vector3[] trajectoryVerts)
    {
        trajectoryMesh.Clear();
        Vector3[] vertices = new Vector3[(lineSegments + 1) * 2];
        int[] triangles = new int[lineSegments * 6 * 2];

        for (int i = 0; i <= lineSegments; i++)
        {
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, trajectoryVerts[i].y, trajectoryVerts[i].x);
            vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, trajectoryVerts[i].y, trajectoryVerts[i].x);

            if (i != lineSegments)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2; 
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;
                
                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }
        }
        trajectoryMesh.vertices = vertices;
        trajectoryMesh.triangles = triangles;
    }

    private Vector3[] CalculateThrowTrajectoryArray()
    {
        Vector3[] throwTrajectoryArray = new Vector3[lineSegments + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= lineSegments; i++)
        {
            float t = (float) i / (float) lineSegments;
            Vector3 newTrajectoryPoint = CalculateTrajectoryPoint(t, maxDistance);
            throwTrajectoryArray[i] = newTrajectoryPoint;
        }
        return throwTrajectoryArray;
    }

    private Vector3 CalculateTrajectoryPoint(float t, float maxDistance)
    {
        float xDistance = t * maxDistance;
        float yDistance = xDistance * Mathf.Tan(radianAngle) - 
                          ((gravity * xDistance * xDistance) / 
                           (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(xDistance, yDistance);
    }
}