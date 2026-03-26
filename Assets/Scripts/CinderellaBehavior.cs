using System;
using System.Collections;
using UnityEngine;

public class Cinderella : CharacterBehavior
{
    [SerializeField]
    private Material DamageZoneMaterial;
    private Material _stockMaterial;
    
    public override void Attack(Case targetCase, CharacterBehavior enemy)
    {
        if ((Math.Abs(CharacterCase._column - targetCase._column) == 1 || Math.Abs(CharacterCase._row - targetCase._row) == 1) && !(Math.Abs(CharacterCase._column - targetCase._column) >= 1 && Math.Abs(CharacterCase._row - targetCase._row) >= 1))
        {
            enemy.ReceiveDamage(Damage);
            StartCoroutine(ShowAttackZone(targetCase));
        }
    }
    
    public override void ReceiveDamage(int damage)
    {
        Life -= damage;
        GameManager.playerHealth.text = "Player health : " + Life;
        if (Life <= 0)
        {
            GameManager.EndGame(false);
        }
    }
    
    public override void ShowAttack()
    {
        
    }

    private IEnumerator ShowAttackZone(Case caseToChange)
    {
        _stockMaterial = caseToChange.GetComponent<MeshRenderer>().material;
        caseToChange.GetComponent<MeshRenderer>().material = DamageZoneMaterial;
        yield return new WaitForSeconds(0.5f);
        caseToChange.GetComponent<MeshRenderer>().material = _stockMaterial;
    }
}
