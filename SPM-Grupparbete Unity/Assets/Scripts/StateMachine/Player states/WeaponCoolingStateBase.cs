//Simon Canbäck, sica4801

using UnityEngine;

//yes, this kind of recursive definition is a nightmare and i don't know why it works. don't touch it --Simon
public abstract class WeaponCoolingStateBase<TIdleState, TFiringState> : WeaponStateBase 
    where TIdleState : WeaponIdleStateBase<TIdleState, TFiringState>
    where TFiringState : WeaponFiringStateBase<TIdleState, TFiringState>
{
    private float cooldownDelayTimer;

    protected new PlayerWeapon Armament => GetMemberInParent((PlayerWeapon)armament);

    public override void Enter()
    {
        cooldownDelayTimer = 0.0f;
    }

    public override void Exit()
    {
    }

    public override void HandleFixedUpdate()
    {
        cooldownDelayTimer += Time.fixedDeltaTime;

        //don't start cooldown until weapon has "sizzled" for a bit
        if (cooldownDelayTimer < Armament.WeaponCooldownTimerStart)
            return;

        Armament.CoolWeapon();

        if (Armament.WeaponCurrentHeat <= 0.0f)
            stateMachine.Transition<TIdleState>();
    }

    public override void HandleUpdate()
    {
    }
}
