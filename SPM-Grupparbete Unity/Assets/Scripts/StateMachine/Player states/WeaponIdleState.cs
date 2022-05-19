//Simon Canbäck, sica4801
using Player;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/WeaponIdleState")]
public class WeaponIdleState : WeaponStateBase
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void HandleFixedUpdate()
    {
        if (Controller.IsShooting)
        {
            stateMachine.Transition<WeaponFiringState>();
            return;
        }

        Armament.CoolWeapon();
    }

    public override void HandleUpdate()
    {
    }
}
