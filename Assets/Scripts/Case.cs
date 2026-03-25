using UnityEngine;

public class Case : MonoBehaviour
{
    public int _column, _row;

    public int Cost;

    public bool BlockPlayer, Occupied;
    
    [SerializeField]
    private GameManager _gameManager;
    
    public Material baseMaterial;
    
    public void Init(int NewColumn, int NewRow, int indice, GameManager gameManager)
    {
        _column = NewColumn;
        _row = NewRow;
        transform.position = new Vector3(_column*11, 1, _row*11);
        //debug
        name = "Case " + indice;
        _gameManager = gameManager;
        if (BlockPlayer)
        {
            baseMaterial = gameManager.blockCaseMaterial;
        }
        else if (baseMaterial == null)
        {
            baseMaterial = gameManager.unselectedCaseMaterial;
        }
        GetComponent<MeshRenderer>().material = baseMaterial;
        
    }

    public void OnMouseDown()
    {
        if (BlockPlayer != true)
        {
            _gameManager.SetNewGoal(this);
        }
    }

    public void ShowBaseMaterial()
    {
        GetComponent<MeshRenderer>().material = baseMaterial;
    }
}
