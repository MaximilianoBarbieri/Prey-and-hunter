using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    Dictionary<Enum, State> _allStates = new Dictionary<Enum, State>(); //hacerlo generico el primer parametro
    State _currentState;

    public void Update()
    {
        _currentState?.OnUpdate();
    }

    public void AddState(Enum name, State state)
    {
        if (!_allStates.ContainsKey(name))
        {
            _allStates.Add(name, state);
            state.stateMachine = this;
        }
        else
        {
            _allStates[name] = state;
        }
    }

    public String getCurrentState()
    {
        return _currentState.ToString();
    }

    public void ChangeState(Enum name) //Para ir cambiando de estados, los vamos pidiendo
    {
        _currentState?.OnExit();
        if (_allStates.ContainsKey(name)) _currentState = _allStates[name];
        _currentState?.OnEnter();
    }
}
