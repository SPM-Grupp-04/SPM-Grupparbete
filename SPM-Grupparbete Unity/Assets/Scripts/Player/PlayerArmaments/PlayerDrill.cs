//Primary authors:
//Simon Canbäck
//
using System;
using UnityEngine;

public class DrillBeam : PlayerBeamArmamentBase
{
    [SerializeField] private StateMachine laserStateMachine;
    protected override bool CanShoot => !(laserStateMachine.CurrentState is LaserWeaponFiringState);
    public override bool IsActivated => GetComponentInParent<PlayerController>().IsDrilling;
}

