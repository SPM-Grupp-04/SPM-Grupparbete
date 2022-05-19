//Primary authors:
//Simon Canbäck
//
using System;
using UnityEngine;

public class PlayerDrill : PlayerBeamArmamentBase
{
    [SerializeField] private ParticleSystem drillRing;
    [SerializeField] private ParticleSystem drillEmission;

    protected override bool CanShoot => !(transform.parent.gameObject.GetComponentInChildren<StateMachine>().CurrentState is WeaponFiringState);

    void FixedUpdate()
    {
        if (CanShoot && GetComponentInParent<PlayerController>().IsDrilling)
            Shoot();
    }
}
