using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    private Enemy _enemy;
    private Player _player;
    
    public Chase(Enemy enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (_enemy.InFieldOfView())
        { //TODO: Alertar a todos los enemy
            _enemy.MoveTo(_player.transform.position);
        }
        else
        {
            stateMachine.ChangeState(EnemyState.ReturnToPatrol);
        }
    }
}
