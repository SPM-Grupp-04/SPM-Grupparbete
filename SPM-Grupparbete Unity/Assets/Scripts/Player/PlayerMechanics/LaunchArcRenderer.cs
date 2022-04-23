using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArcRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private float velocity;
    [SerializeField] private float angle;
    [SerializeField] private int lineSegments = 10;

    private float gravity;
    private float radianAngle;

    private void OnValidate()
    {
        if (lineRenderer != null && Application.isPlaying)
        {
            RenderThrowTrajectory();
        }
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics.gravity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        RenderThrowTrajectory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RenderThrowTrajectory()
    {
        lineRenderer.positionCount = lineSegments + 1;
        lineRenderer.SetPositions(CalculateThrowTrajectoryArray());
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
