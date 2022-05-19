//Simon Canbäck, sica4801
using Player;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/WeaponFiringState")]
public class WeaponFiringState : WeaponStateBase
{
    public override void Enter()
    {
        Armament.VFXLineRenderer.enabled = true;
        Armament.Shoot(); //makes sure it fires on the same frame as the state is entered
    }

    public override void Exit()
    {
        Armament.VFXLineRenderer.enabled = false;
        Armament.ClearVFX();
    }

    public override void HandleFixedUpdate()
    {
        if (!Controller.IsShooting)
        {
            stateMachine.Transition<WeaponIdleState>();
            return;
        }

        Armament.Shoot();

        if (Armament.WeaponCurrentHeat >= 100.0f)
            stateMachine.Transition<WeaponCoolingState>();
    }

    public override void HandleUpdate()
    {
    }
}
