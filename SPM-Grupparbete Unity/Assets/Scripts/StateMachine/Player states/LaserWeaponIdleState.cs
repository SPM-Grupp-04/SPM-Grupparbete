//Simon Canbäck, sica4801

using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/LaserWeaponIdleState")]
public class LaserWeaponIdleState : WeaponIdleStateBase<LaserWeaponIdleState, LaserWeaponFiringState>
{
    protected new PlayerWeapon Armament => GetMemberInParent((PlayerWeapon)armament);

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        Armament.CoolWeapon();
    }

    public override void HandleUpdate()
    {
    }
}

