//Simon Canbäck, sica4801

using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/DrillWeaponIdleState")]
public class DrillWeaponIdleState : WeaponIdleStateBase<DrillWeaponIdleState, DrillWeaponFiringState>
{
    public override void HandleUpdate()
    {
    }
}
