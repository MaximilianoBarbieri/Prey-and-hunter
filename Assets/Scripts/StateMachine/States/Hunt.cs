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
      
   }

   public override void OnExit()
   {

   }

   public override void OnUpdate()
   {        
      _enemy.Hunt(); //Ir hacia el ultimo rastro del PJ
   }
}
