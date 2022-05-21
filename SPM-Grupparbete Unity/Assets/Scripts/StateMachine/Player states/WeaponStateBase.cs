//Simon Canbäck, sica4801

using Player;
using UnityEngine;

public abstract class WeaponStateBase : ControllerStateBase
{
    protected PlayerBeamArmamentBase armament;

    protected PlayerBeamArmamentBase Armament => GetMemberInParent(armament);

    protected virtual bool IsActivated => Armament.IsActivated;

    public new void Initialize(StateMachine stateMachine)
    {
        this.Initialize(stateMachine, Armament);
    }

    public virtual void Initialize(StateMachine stateMachine, PlayerBeamArmamentBase weapon)
    {
        base.Initialize(stateMachine);
        this.armament = weapon;
    }
}
