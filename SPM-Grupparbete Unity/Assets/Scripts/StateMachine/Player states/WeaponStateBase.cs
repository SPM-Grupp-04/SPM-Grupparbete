//Simon Canbäck, sica4801

using Player;
using UnityEngine;

public abstract class WeaponStateBase : ControllerStateBase
{
    private PlayerBeamArmamentBase armament;
    private Animator animator;

    protected PlayerBeamArmamentBase Armament => GetMemberInParent(armament);
    public Animator WeaponAnimator
    {
        get
        {
            if (animator == null)
                animator = Controller.GetComponentInChildren<Animator>();

            return animator;
        }
    }
    protected virtual bool IsActivated => Armament.IsActivated;
}
