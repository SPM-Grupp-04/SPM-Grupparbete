using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter))]
public class AxelLaunchArcMesh : MonoBehaviour
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

    private float gravity;
    private float radianAngle;

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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (enterDynamiteThrowModeInputAction.IsPressed())
        {
            TrajectoryPrediction();
        }
        else
        {
            trajectoryMesh.Clear();
        }
    }

    private void TrajectoryPrediction()
    {
        //Debug.Log(increaseTrajectoryArcInputAction.ReadValue<float>());
        velocity += increaseTrajectoryArcInputAction.ReadValue<float>() * trajectoryArcIncreaseSpeed;
        angle += increaseTrajectoryArcInputAction.ReadValue<float>() * 0.005f;
        RenderThrowTrajectoryMesh(CalculateThrowTrajectoryArray());
    }

    private void DisableTrajectoryPrediction()
    {
        velocity = 0.0f;
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
            throwTrajectoryArray[i] = CalculateTrajectoryPoint(t, maxDistance);
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