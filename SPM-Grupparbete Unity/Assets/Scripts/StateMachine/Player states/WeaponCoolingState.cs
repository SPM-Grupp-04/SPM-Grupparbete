//Simon Canbäck, sica4801

using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/WeaponCoolingState")]
public class WeaponCoolingState : WeaponStateBase
{
    private float cooldownDelayTimer;

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
            stateMachine.Transition<WeaponIdleState>();
    }

    public override void HandleUpdate()
    {
    }
}
