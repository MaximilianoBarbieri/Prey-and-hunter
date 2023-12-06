using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPatrol : State
{
    private Enemy _enemy;

    public ReturnToPatrol(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
        _enemy.ReturnToPatrol(); //obtengo la ruta de enemy a su origen de patrullaje
        _enemy._currentNodePatrol = 0;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (_enemy.InFieldOfView())
        {
            stateMachine.ChangeState(EnemyState.Chase);
        }
        else if (_enemy.Path.Count > 0) //Viajo hasta el final de la ruta si es que lo hay
        {
            _enemy.TravelPath();
        }
        else if (Vector3.Distance(_enemy.transform.position, _enemy.patrolNodes[0].transform.position) <= 0.1f) //si estoy cerca de mi primer nodo de patrullaje, cambio a patrol
        {
            stateMachine.ChangeState(EnemyState.Patrol);
        }
        else
        {
            _enemy.MoveTo(_enemy._currentNode.transform.position);
        }
    }
}