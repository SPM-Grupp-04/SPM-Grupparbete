//Author: Simon Canbäck, sica4801

using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/LaserWeaponFiringState")]
public class LaserWeaponFiringState : WeaponFiringStateBase<LaserWeaponIdleState, LaserWeaponFiringState>
{
    private PlayerWeapon armament;
    protected new PlayerWeapon Armament => GetMemberInParent(armament);

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        if (Armament.IsOverheated)
        {
            stateMachine.Transition<LaserWeaponCoolingState>();
            return;
        }
    }

    public override void HandleUpdate()
    {
    }
}
