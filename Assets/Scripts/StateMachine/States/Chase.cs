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
        if (_enemy.InFieldOfView()) // si esta en su vision que lo siga y mientras tanto que le avise a los otros enemy
        { 
            _enemy.MoveTo(_player.transform.position); //Mientras el enemigo lo sigue
            //TODO: activar evento para que los demas Enemy pasen a estado Hunt
        }
        else
        {
            stateMachine.ChangeState(EnemyState.ReturnToPatrol);
        }
    }
}
