using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Realizar sistema A* para buscar el nodo 0, evitando que traspase las paredes
public class ReturnToPatrol : State
{
    private Enemy _enemy;

    public ReturnToPatrol(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
        _enemy._currentNodePatrol = 0;
    }

    public override void OnUpdate()
    {
        if (_enemy.InFieldOfView()) stateMachine.ChangeState(EnemyState.Chase);

        _enemy.MoveTo(_enemy.patrolNodes[0].transform.position);
        if (Vector3.Distance(_enemy.transform.position, _enemy.patrolNodes[0].transform.position) < 0.01f)
            stateMachine.ChangeState(EnemyState.Patrol);
    }
}