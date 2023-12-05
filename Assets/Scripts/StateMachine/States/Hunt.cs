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
      Debug.Log("Enter Hunt" + _enemy.Path.Count);
      _enemy.Hunt(); //obtengo la ruta de enemy al player
      Debug.Log("Post Hunt" + _enemy.Path.Count);
   }

   public override void OnExit()
   {

   }

   public override void OnUpdate()
   {        
      
      if (_enemy.Path.Count > 0) //Viajo hasta el final de la ruta si es que lo hay
      {
         Debug.Log("OnUpdate TravelPath " + _enemy.Path.Count);
         _enemy.TravelPath();
      }
      else
      {
         Debug.Log("OnUpdate ELSE TravelPath " + _enemy.Path.Count);
      }
   }
}
