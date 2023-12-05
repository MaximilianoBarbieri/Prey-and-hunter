using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State
{
    private Enemy _enemy;
    public Patrol(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
       //_enemy.MoveTo(_enemy.patrolNodes[0].transform.position);
    }

    public override void OnExit(){}

    public override void OnUpdate()
    {
        Debug.Log("Esto en Patrol STATE");
        if (_enemy.InFieldOfView())
        {
            stateMachine.ChangeState(EnemyState.Chase);
        }
        else
        {
            _enemy.PatrolAStar();
        }
    }
}