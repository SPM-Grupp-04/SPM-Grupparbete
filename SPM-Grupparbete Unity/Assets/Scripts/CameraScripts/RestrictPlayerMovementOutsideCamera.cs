using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictPlayerMovementOutsideCamera : MonoBehaviour
{
    private static RestrictPlayerMovementOutsideCamera instance;

    public static RestrictPlayerMovementOutsideCamera Instance
    {
        get { return instance; }
    }

    [SerializeField] private BoxCollider wallCollider;

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
    }

    public void MoveInvisibleWallToBlockPlayer(Vector3 playerToBlockPosition, Quaternion worldRotation)
    {
        transform.position = playerToBlockPosition;
        transform.rotation = worldRotation;
        wallCollider.enabled = true;
    }

    public void ResetInvisibleWall()
    {
        wallCollider.enabled = false;
    }
}
