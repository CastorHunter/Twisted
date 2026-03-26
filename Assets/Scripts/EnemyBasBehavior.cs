using System;
using System.Collections;
using UnityEngine;

public class EnemyBasBehavior : CharacterBehavior
{
    [SerializeField]
    private Material DamageZoneMaterial;
    
    private Material _stockMaterial;
    
    private void Start()
    {
        Movement += 1;
    }
    
    public override void Attack(Case targetCase,  CharacterBehavior player)
    {
        if ((Math.Abs(CharacterCase._column - targetCase._column) == 1 || Math.Abs(CharacterCase._row - targetCase._row) == 1) && !(Math.Abs(CharacterCase._column - targetCase._column) >= 1 && Math.Abs(CharacterCase._row - targetCase._row) >= 1))
        {
            player.ReceiveDamage(Damage);
            StartCoroutine(ShowAttackZone(targetCase));
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
    
    private IEnumerator ShowAttackZone(Case caseToChange)
    {
        _stockMaterial = caseToChange.GetComponent<MeshRenderer>().material;
        caseToChange.GetComponent<MeshRenderer>().material = DamageZoneMaterial;
        yield return new WaitForSeconds(0.5f);
        caseToChange.GetComponent<MeshRenderer>().material = _stockMaterial;
    }
}
