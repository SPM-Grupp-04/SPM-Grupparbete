//Simon Canbäck, sica4801

using UnityEngine;

namespace Player
{
    public abstract class ControllerStateBase : StateBase
    {
        private PlayerController controller;

        public PlayerController Controller => GetMemberInParent(controller);

        protected T GetMemberInParent<T>(T member) where T : MonoBehaviour
        {
            member = stateMachine.gameObject.GetComponentInParent<T>();

            if (member == null)
                throw new System.ArgumentNullException("Controller", "No " + member.GetType().Name + " found. Set one or ensure this instance is in the hierarchy of one.");

            return member;
        }
    }
}