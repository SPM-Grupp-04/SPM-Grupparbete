using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Main Author: Axel Ingelsson Fredler

[RequireComponent(typeof(MeshFilter))]
public class LaunchArcMesh : MonoBehaviour
{
    [SerializeField] private float meshWidth;
    
    [SerializeField] [Range(1.0f, 10.0f)] private float velocity = 1.0f;
    [SerializeField] private float angle = 45.0f;
    [SerializeField] [Range(1.0f, 5.0f)] private float trajectoryArcIncreaseSpeed = 2.0f;
    [SerializeField] private float trajectoryArcAngleIncreaseSpeed = 1.0f;
    [SerializeField] private int lineSegments = 10;

    [SerializeField] private LayerMask collisionLayerMask;

    private DynamiteThrow dynamiteThrowScript;
    
    private Mesh trajectoryMesh;

    private bool isDynamiteThrowModeEntered;
    private bool increaseDynamiteArc;
    private bool keepDynamiteArcLength;
    
    private float gravity;
    private float radianAngle;

    private Vector3 newTrajectoryPoint;

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

    private void Awake()
    {
        trajectoryMesh = GetComponent<MeshFilter>().mesh;
        gravity = Mathf.Abs(Physics.gravity.y);
        dynamiteThrowScript = GetComponent<DynamiteThrow>();
    }
    
    private void Update()
    {
        if (isDynamiteThrowModeEntered)
        {
            RenderThrowTrajectoryMesh(CalculateThrowTrajectoryArray());
            if (increaseDynamiteArc)
            {
                TrajectoryArcIncrease();
            }
            else
            {
                TrajectoryArcDecrease();
            }
        } 
        else if (velocity <= 1.0f)
        {
            isDynamiteThrowModeEntered = false;
            DisableTrajectoryArc();
        }
    }

    public void TrajectoryInput(InputAction.CallbackContext trajectoryInputValue)
    {
        if (trajectoryInputValue.performed)
        {
            isDynamiteThrowModeEntered = true;
            increaseDynamiteArc = true;
        }
        else if (trajectoryInputValue.canceled)
        {
            dynamiteThrowScript.ThrowDynamite();
            increaseDynamiteArc = false;
        }
    }

    private void TrajectoryArcIncrease()
    {
        velocity += (trajectoryArcIncreaseSpeed + trajectoryArcIncreaseSpeed) * Time.deltaTime;
        velocity = Mathf.Clamp(velocity, 1.0f, 10.0f);
    }

    private void TrajectoryArcDecrease()
    {
        velocity -= trajectoryArcIncreaseSpeed * Time.deltaTime;
        velocity = Mathf.Clamp(velocity, 1.0f, 10.0f);
    }

    private void DisableTrajectoryArc()
    {
        velocity = 1.0f;
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
            newTrajectoryPoint = CalculateTrajectoryPoint(t, maxDistance);

            if (Physics.OverlapSphere(transform.position  + (transform.forward * newTrajectoryPoint.magnitude), 0.1f, collisionLayerMask).Length > 0)
            {
                break;
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(  transform.position  + (transform.forward * newTrajectoryPoint.magnitude), 0.1f);
    }
}