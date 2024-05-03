using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }

    public void Initialize(EnemyState _enemyState)
    {
        currentState = _enemyState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _changeState)
    {
        currentState.Exit();
        currentState = _changeState;
        currentState.Enter();
    }
}
