using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : State
{
    private Enemy _enemy;

    public Hunt(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
        _enemy.Hunt(); //obtengo la ruta de enemy al player
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (_enemy.Path.Count > 0) //Viajo hasta el final de la ruta si es que lo hay
        {
            _enemy.TravelPath();
        }
    }
}