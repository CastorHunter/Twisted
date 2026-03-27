using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    [SerializeField] private List<GameObject> Heroes, Enemies;

    [SerializeField] private GameObject EndTurnButton, PlayerGhost, AttackZone;
    public TextMeshProUGUI playerHealth, enemyHealth, actionPoints, victoryShowUp, tip;
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
                character.GetComponent<CharacterBehavior>().CharacterCase = board[14];
                character.transform.position = board[14].transform.position;
                character.GetComponent<CharacterBehavior>().GameManager = this;
            }
            foreach (GameObject character in Enemies) //debug
            {
                //Instantiate(character, new Vector3(0, 1, 0), Quaternion.identity);
                character.GetComponent<CharacterBehavior>().CharacterCase = board[114];
                character.transform.position = board[114].transform.position;
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
        tip.enabled = false;
        AttackZone.SetActive(false);
        PlayerGhost.transform.position = Heroes[0].transform.position;
        PlayerGhost.SetActive(false);
        LaunchGame();
    }

    public void SetNewGoal(Case newGoal)
    {
        PlayerGhost.SetActive(false);
        AttackZone.transform.position = Heroes[0].transform.position;
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
                PlayerGhost.SetActive(true);
                if (_goalCase.Occupied ==  false)
                {
                    AttackZone.transform.position = _goalCase.transform.position;
                }
                else
                {
                    AttackZone.transform.position = Heroes[0].transform.position;
                }
                PlayerGhost.transform.position = _goalCase.transform.position;
                Heroes[0].GetComponent<CharacterBehavior>().SetCanMove(_goalCase);
                actionPoints.text = 0.ToString();
                tip.enabled = true;
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
        AttackZone.SetActive(false);
        PlayerGhost.SetActive(false);
        Heroes[0].GetComponent<CharacterBehavior>().HideMovement();
        EndTurnButton.GetComponent<Button>().interactable = false;
        if (_goalCase != Heroes[0].GetComponent<CharacterBehavior>().CharacterCase)
        {
            Heroes[0].GetComponent<CharacterBehavior>().Move();
            PlayerGhost.transform.position = Heroes[0].transform.position;
            AttackZone.transform.position = Heroes[0].transform.position;
        }
        Heroes[0].GetComponent<Cinderella>().Attack(Enemies[0].GetComponent<CharacterBehavior>().CharacterCase, Enemies[0].GetComponent<CharacterBehavior>());
        if (_gameEnded == false)
        {
            StartCoroutine(PlayEnemyTurn());
        }
    }
    
    private void PlayPlayerTurn()
    {
        AttackZone.SetActive(true);
        actionPoints.text = 1.ToString();
        Heroes[0].GetComponent<CharacterBehavior>().ShowMovement();
        EndTurnButton.GetComponent<Button>().interactable = true;
    }
    
    private IEnumerator PlayEnemyTurn()
    {
        tip.enabled = false;
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
