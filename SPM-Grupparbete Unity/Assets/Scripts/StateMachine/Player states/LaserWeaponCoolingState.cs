//Author: Simon Canbäck, sica4801

using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Player/LaserWeaponCoolingState")]
public class LaserWeaponCoolingState : WeaponCoolingStateBase<LaserWeaponIdleState, LaserWeaponFiringState>
{

}
