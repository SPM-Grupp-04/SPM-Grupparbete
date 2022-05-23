//Simon Canbäck, sica4801
using Player;
using System;
using UnityEngine;

//yes, this kind of recursive definition is a nightmare and i don't know why it works. don't touch it --Simon
public abstract class WeaponIdleStateBase<TIdleState, TFiringState> : WeaponStateBase 
    where TIdleState : WeaponIdleStateBase<TIdleState, TFiringState> 
    where TFiringState : WeaponFiringStateBase<TIdleState, TFiringState>
{
    public override void Enter()
    {
        WeaponAnimator.SetBool("Idle", true);
    }

    public override void Exit()
    {
        WeaponAnimator.SetBool("Idle", false);
    }

    public override void HandleFixedUpdate()
    {
        if (IsActivated)
        {
            stateMachine.Transition<TFiringState>();
            return;
        }
    }

    public override void HandleUpdate()
    {
    }
}
