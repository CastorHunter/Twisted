using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public int Life, Damage, Appearance, Movement;
    public GameManager GameManager;
    public Case CharacterCase, CheckingCase, CaseToMove;
    
    public virtual void Attack(Case targetCase, CharacterBehavior target)
    {
        
    }

    public virtual void ReceiveDamage(int damage)
    {
        Life -= damage;
        if (Life <= 0)
        {
            GameManager.EndGame(true);
        }
    }
    
    public virtual void Move()
    {
        if (CaseToMove != null)
        {
            CharacterCase.Occupied = false;
            var pathToFollow = FindShortestPath(CharacterCase, CaseToMove, GameManager.boardManager.Cases);
            var loopEnd = 0;
            if (Movement+1 < pathToFollow.Count)
            {
                loopEnd = Movement+1;
            }
            else
            {
                loopEnd = pathToFollow.Count;
            }

            if (pathToFollow[pathToFollow.Count-1].Occupied)
            {
                loopEnd -= 1;
            }
            for (int i=0; i <loopEnd; i++)
            {
                CharacterCase = pathToFollow[i];
                transform.position = new Vector3(pathToFollow[i]._column*11, 1, pathToFollow[i]._row*11);
            }
            CharacterCase.Occupied = true;
        }
    }

    public virtual bool CheckCanMove(Case targetCase)
    {
        if (targetCase.BlockPlayer == true)
        {
            return false;
        }
        CheckingCase = CharacterCase;
        var pathToFollow = FindShortestPath(CheckingCase, targetCase, GameManager.boardManager.Cases);
        var loopEnd = 0;
        if (Movement+1 < pathToFollow.Count)
        {
            loopEnd = Movement+1;
        }
        else
        {
            loopEnd = pathToFollow.Count;
        }
        for (int i=0; i < loopEnd; i++)
        {
            CheckingCase = pathToFollow[i];
            if (CheckingCase == targetCase)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void SetCanMove(Case targetCase)
    {
        if (CheckCanMove(targetCase) && targetCase != CaseToMove)
        {
            CaseToMove = targetCase;
        }
    }

    public void ShowMovement()
    {
        foreach (Case c in GameManager.boardManager.Cases)
        {
            if (CheckCanMove(c))
            {
                c.GetComponent<MeshRenderer>().material = GameManager.movementCaseMaterial;
            }
        }
        ShowAttack();
    }

    public void HideMovement()
    {
        foreach (Case c in GameManager.boardManager.Cases)
        {
            c.ShowBaseMaterial();
        }
    }

    public virtual void ShowAttack()
    {
        
    }

    public List<Case> FindShortestPath(Case start, Case end, List<Case> board)
    {
        foreach (Case c in board)
        {
            c.Cost = Math.Abs(end._column - c._column) + Math.Abs(end._row - c._row);
        }
        var casesToCheck = new List<Case>();
        casesToCheck = board; //Create a copy of the board where will be deleted Cases that the player can not use to reach the goal
        var currentPath = new List<Case>(); //Create a list of Case which will stock the cases the player uses to reach the goal
        currentPath.Add(start);
        var currentCase = start;
        var bestPath = new List<Case>();
        var newCurrentCase = start;
        var totalCost = 0;
        var bestCost = 0;
        
            while (currentCase != end)
            {
                foreach (Case c in casesToCheck)
                {
                    if (true) //c.BlockPlayer != 
                    {
                        if ((Math.Abs(currentCase._column - c._column) == 1 || Math.Abs(currentCase._row - c._row) == 1) && !(Math.Abs(currentCase._column - c._column) >= 1 && Math.Abs(currentCase._row - c._row) >= 1))
                        {
                            if (c.Cost < newCurrentCase.Cost || newCurrentCase == currentCase) //(c.Cost < newCurrentCase.Cost || newCurrentCase == currentCase)
                            {
                                newCurrentCase = c;
                                totalCost += c.Cost;
                                //print(c);
                            }
                        }
                    }
                }
                
                if (totalCost < bestCost || bestCost == 0)
                {
                    bestCost = totalCost;
                    bestPath = currentPath;
                }
                
                if (currentCase != newCurrentCase)
                {
                    currentCase = newCurrentCase;
                    currentPath.Add(currentCase);
                }
                else
                {
                    casesToCheck.Remove(currentCase);
                    currentPath.Clear();
                    newCurrentCase = start;
                    currentCase = start;
                    totalCost = 0;
                }
            }

            return bestPath;
    }
}
