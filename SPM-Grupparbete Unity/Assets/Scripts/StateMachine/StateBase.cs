//Simon Canbäck, sica4801

using UnityEngine;

public abstract class StateBase : ScriptableObject
{
    protected StateMachine stateMachine;

    public virtual void Initialize(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void HandleUpdate();
    public abstract void HandleFixedUpdate();
}
