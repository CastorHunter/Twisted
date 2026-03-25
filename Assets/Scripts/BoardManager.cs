using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public List<Case>  Cases;
    public GameObject CasePrefab;
    
    [SerializeField]
    private GameManager _gameManager;

    public void CreateBoard(int Columns, int Rows)
    {
        if (Cases.Count == 0)
        {
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    var newCase = Instantiate(CasePrefab).GetComponent<Case>();
                    newCase.Init(i, j, Cases.Count+1, _gameManager);
                    newCase.GetComponent<MeshRenderer>().material = _gameManager.unselectedCaseMaterial;
                    Cases.Add(newCase);
                }
            }
            var camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            camera.transform.position = new Vector3(Columns*5.5f-5, Rows*10+10, Rows*5.5f-5);
        }
        else
        {
            Debug.Log("Please delete the current board first");
        }
    }

    public void DeleteBoard(float casesToDelete = 0)
    {
        casesToDelete = Cases.Count;
        for (int i = 0; i < casesToDelete; i++)
        {
            DestroyImmediate(Cases[0].gameObject);
            Cases.Remove(Cases[0]);
        }
    }
}
