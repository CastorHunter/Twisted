using System;
using UnityEngine;

public class EnemyBasBehavior : CharacterBehavior
{
    private void Start()
    {
        Movement += 1;
    }
    
    public override void Attack(Case targetCase,  CharacterBehavior player)
    {
        if ((Math.Abs(CharacterCase._column - targetCase._column) == 1 || Math.Abs(CharacterCase._row - targetCase._row) == 1) && !(Math.Abs(CharacterCase._column - targetCase._column) >= 1 && Math.Abs(CharacterCase._row - targetCase._row) >= 1))
        {
            player.ReceiveDamage(Damage);
        }
    }

    public override bool CheckCanMove(Case targetCase)
 {
     if (targetCase.BlockPlayer == true)
     {
         return false;
     }
     CheckingCase = CharacterCase;
     var pathToFollow = FindShortestPath(CheckingCase, targetCase, GameManager.boardManager.Cases);
     for (int i=0; i < 50; i++)
     {
         CheckingCase = pathToFollow[i];
         if (CheckingCase == targetCase)
         {
             return true;
         }
     }
     return false;
 }
}
