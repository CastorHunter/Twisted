using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    [SerializeField] private List<GameObject> Heroes, Enemies;

    [SerializeField] private GameObject EndTurnButton;
    public TextMeshProUGUI playerHealth, enemyHealth, actionPoints, victoryShowUp;
    [SerializeField] private Image blurImage;
    
    private Case _goalCase;
    
    public Material unselectedCaseMaterial, selectedCaseMaterial, movementCaseMaterial, blockCaseMaterial;
    
    private bool _gameEnded;
    
    private void Start()
    {
        var board = boardManager.Cases;
        _goalCase = board[0]; //debug
        
        if (Heroes.Count > 0)
        {
            foreach (GameObject character in Heroes) //debug
            {
                //Instantiate(character, new Vector3(0, 1, 0), Quaternion.identity);
                character.GetComponent<CharacterBehavior>().CharacterCase = board[0];
                character.GetComponent<CharacterBehavior>().GameManager = this;
            }
            foreach (GameObject character in Enemies) //debug
            {
                //Instantiate(character, new Vector3(0, 1, 0), Quaternion.identity);
                character.GetComponent<CharacterBehavior>().CharacterCase = board[29];
                character.GetComponent<CharacterBehavior>().GameManager = this;
            }
        }
        else
        {
            Debug.Log("No heroes found");
        }
        enemyHealth.text = ("Enemy health : " + Enemies[0].GetComponent<CharacterBehavior>().Life);
        playerHealth.text = ("Player health : " + Heroes[0].GetComponent<CharacterBehavior>().Life);
        actionPoints.text = 1.ToString();
        victoryShowUp.enabled = false;
        blurImage.enabled = false;
        LaunchGame();
    }

    public void SetNewGoal(Case newGoal)
    {
        if (_goalCase != newGoal)
        {
            if (Heroes[0].GetComponent<CharacterBehavior>().CheckCanMove(_goalCase))
            {
                _goalCase.GetComponent<MeshRenderer>().material = movementCaseMaterial;
            }
            else
            {
                _goalCase.ShowBaseMaterial();
            }
            
            if (Heroes[0].GetComponent<CharacterBehavior>().CheckCanMove(newGoal))
            {
                _goalCase = newGoal;
                _goalCase.GetComponent<MeshRenderer>().material = selectedCaseMaterial;
                Heroes[0].GetComponent<CharacterBehavior>().SetCanMove(_goalCase);
                actionPoints.text = 0.ToString();
            }
        }
        else
        {
            _goalCase.GetComponent<MeshRenderer>().material = selectedCaseMaterial;
        }
    }

    private void LaunchGame()
    {
        PlayPlayerTurn();
    }

    public void EndTurn()
    {
        Heroes[0].GetComponent<CharacterBehavior>().HideMovement();
        EndTurnButton.GetComponent<Button>().interactable = false;
        if (_goalCase != Heroes[0].GetComponent<CharacterBehavior>().CharacterCase)
        {
            Heroes[0].GetComponent<CharacterBehavior>().Move();
        }
        Heroes[0].GetComponent<Cinderella>().Attack(Enemies[0].GetComponent<CharacterBehavior>().CharacterCase, Enemies[0].GetComponent<CharacterBehavior>());
        if (_gameEnded == false)
        {
            StartCoroutine(PlayEnemyTurn());
        }
    }
    
    private void PlayPlayerTurn()
    {
        actionPoints.text = 1.ToString();
        Heroes[0].GetComponent<CharacterBehavior>().ShowMovement();
        EndTurnButton.GetComponent<Button>().interactable = true;
    }
    
    private IEnumerator PlayEnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        Enemies[0].GetComponent<CharacterBehavior>().SetCanMove(Heroes[0].GetComponent<CharacterBehavior>().CharacterCase);
        Enemies[0].GetComponent<CharacterBehavior>().Move();
        Enemies[0].GetComponent<CharacterBehavior>().Attack(Heroes[0].GetComponent<CharacterBehavior>().CharacterCase, Heroes[0].GetComponent<CharacterBehavior>());
        yield return new WaitForSeconds(1f);
        if (_gameEnded == false)
        {
            PlayPlayerTurn();
        }
    }

    public void EndGame(bool victory)
    {
        if (victory)
        {
            _gameEnded = true;
            victoryShowUp.enabled = true;
            blurImage.enabled = true;
            victoryShowUp.GetComponent<TextMeshProUGUI>().text = "VICTORY";
        }
        else
        {
            _gameEnded = true;
            victoryShowUp.enabled = true;
            blurImage.enabled = true;
            victoryShowUp.GetComponent<TextMeshProUGUI>().text = "DEFEAT";
        }
    }
}
