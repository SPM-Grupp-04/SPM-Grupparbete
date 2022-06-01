using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main author: Axel Ingelsson Fredler

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;

    private PlayerController playerController;

    private Vector3 velocity;

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        AnimatePlayer();
    }
    
    private void AnimatePlayer()
    {
        AnimatePlayerMovement();
        AnimatePlayerShootingAndDrilling();
    }

    private void AnimatePlayerMovement()
    {
        velocity = playerController.PlayerVelocity;
        
        if (velocity != Vector3.zero)
        {
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("Moving", true);

            float velocityInDirectionOfWherePlayerIsFacing = Vector3.Dot(transform.forward, velocity.normalized);
            float velocityInDirectionOfPlayersSides = Vector3.Dot(transform.right, velocity.normalized);

            playerAnimator.SetFloat("ForwardAndBackwardMovement", velocityInDirectionOfWherePlayerIsFacing);
            playerAnimator.SetFloat("SidewaysMovement", velocityInDirectionOfPlayersSides);
        }
        else
        {
            playerAnimator.SetBool("Moving", false);
            playerAnimator.SetBool("Idle", true);
        }
    }

    private void AnimatePlayerShootingAndDrilling()
    {
        if (playerController.IsShooting || playerController.IsDrilling)
        {
            playerAnimator.SetBool("IsShooting", true);
        }
        else
        {
            playerAnimator.SetBool("IsShooting", false);   
        }

    }
}
