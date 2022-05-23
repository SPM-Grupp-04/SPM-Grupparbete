//Author: Simon Canbäck, sica4801

//yes, this kind of recursive definition is a nightmare and i don't know why it works. don't touch it --Simon

using UnityEngine;

public abstract class WeaponFiringStateBase<TIdleState, TFiringState> : WeaponStateBase 
    where TIdleState : WeaponIdleStateBase<TIdleState, TFiringState>
    where TFiringState : WeaponFiringStateBase<TIdleState, TFiringState>
{
     private Animator animator;
    public override void Enter()
    {
        Armament.VFXLineRenderer.enabled = true;
        Armament.Shoot(); //makes sure it fires on the same frame as the state is entered
        Armament.PlaySound();
        animator.SetBool("IsShooting", true);
    }

    public override void Exit()
    {
        Armament.VFXLineRenderer.enabled = false;
        Armament.ClearVFX();
        Armament.StopSound();
        animator.SetBool("IsShooting", false);
        
    }

    public override void HandleFixedUpdate()
    {
        if (!IsActivated)
        {
            stateMachine.Transition<TIdleState>();
            return;
        }

        Armament.Shoot();
    }

    public new void Initialize(StateMachine stateMachine)
    {
        base.Initialize(stateMachine);
        animator = Controller.gameObject.GetComponentInChildren<Animator>();
    }
}
