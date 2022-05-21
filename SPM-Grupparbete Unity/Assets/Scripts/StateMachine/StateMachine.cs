using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	[SerializeField] private StateBase[] allowedStates;
	private Dictionary<Type, StateBase> stateDictionary;

	public StateBase CurrentState
	{
		get; private set;
	}

    private void Awake()
    {
		stateDictionary = new Dictionary<Type, StateBase>();
		StateBase state;

		foreach (StateBase v in allowedStates)
		{
			state = Instantiate(v);
			state.Initialize(this);
			stateDictionary.Add(state.GetType(), state);

			if (CurrentState == null)
				CurrentState = state;
		}

		CurrentState.Enter();
    }

    public void Transition<T>() where T : StateBase
	{
		CurrentState.Exit();
		CurrentState = stateDictionary[typeof(T)];
		CurrentState.Enter();
	}

	private void Update()
    {
		CurrentState.HandleUpdate();
    }

    private void FixedUpdate()
    {
		CurrentState.HandleFixedUpdate();
    }
}
