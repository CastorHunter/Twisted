using System;
using UnityEngine;

public class Cinderella : CharacterBehavior
{
    public override void Attack(Case targetCase, CharacterBehavior enemy)
    {
        if ((Math.Abs(CharacterCase._column - targetCase._column) == 1 || Math.Abs(CharacterCase._row - targetCase._row) == 1) && !(Math.Abs(CharacterCase._column - targetCase._column) >= 1 && Math.Abs(CharacterCase._row - targetCase._row) >= 1))
        {
            enemy.ReceiveDamage(Damage);
        }
    }
    
    public override void ReceiveDamage(int damage)
    {
        Life -= damage;
        GameManager.playerHealth.text = "Health : " + Life;
        if (Life <= 0)
        {
            GameManager.EndGame(false);
        }
    }
    
    public override void ShowAttack()
    {
        
    }
}
