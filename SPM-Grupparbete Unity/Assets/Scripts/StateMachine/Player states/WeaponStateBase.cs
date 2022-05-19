//Simon Canbäck, sica4801

using Player;
using UnityEngine;

public abstract class WeaponStateBase : ControllerStateBase
{
    private PlayerWeapon weapon;

    protected PlayerWeapon Armament => GetMemberInParent(weapon);

    public new void Initialize(StateMachine stateMachine)
    {
        this.Initialize(stateMachine, Armament);
    }

    public virtual void Initialize(StateMachine stateMachine, PlayerWeapon weapon)
    {
        base.Initialize(stateMachine);
        this.weapon = weapon;
    }
}
