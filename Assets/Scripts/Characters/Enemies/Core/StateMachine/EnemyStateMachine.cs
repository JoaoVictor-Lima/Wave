using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }

    private Dictionary<System.Type, IEnemyState> _states
        = new Dictionary<System.Type, IEnemyState>();

    public void AddState(IEnemyState state)
    {
        var type = state.GetType();

        if (_states.ContainsKey(type))
        {
            Debug.LogWarning($"State {type} já existe na StateMachine");
            return;
        }

        _states.Add(type, state);
    }

    public void ChangeState<T>() where T : IEnemyState
    {
        var type = typeof(T);

        if (!_states.TryGetValue(type, out var newState))
        {
            Debug.LogError($"State {type} não registrado na StateMachine");
            return;
        }

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
