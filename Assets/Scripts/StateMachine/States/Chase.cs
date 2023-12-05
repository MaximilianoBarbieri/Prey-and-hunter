using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    private Enemy _enemy;
    private Player _player;
    public bool oneShot = true;
    
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
        Debug.Log("Esto en CHASE STATE");
        if (_enemy.InFieldOfView()) _enemy.ChasePlayer();
        /*if (_enemy.InFieldOfView()) // si esta en su vision que lo siga y mientras tanto que le avise a los otros enemy
        { 
            //TODO: Alertar a todos los enemy
            _enemy.MoveTo(_player.transform.position);
        }
        else
        {
            stateMachine.ChangeState(EnemyState.ReturnToPatrol);
        }*/
    }
}
