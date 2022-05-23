//Author: Simon Canbäck, sica4801

using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/DrillWeaponFiringState")]
public class DrillWeaponFiringState : WeaponFiringStateBase<DrillWeaponIdleState, DrillWeaponFiringState>
{
    public override void HandleUpdate()
    {
    }
}
