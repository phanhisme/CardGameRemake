using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameManager _gm;
    private EnemyBehaviour enemyScript;

    public bool isPlayerTurn;
    public bool isEnemyTurn;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        enemyScript = FindObjectOfType<EnemyBehaviour>();
    }

    public void Update()
    {
        if (_gm.turn == GameManager.Turn.Player)
        {
            isPlayerTurn = true;
            isEnemyTurn = false;
        }

        if (_gm.turn == GameManager.Turn.Enemy)
        {
            isEnemyTurn = true;
            isPlayerTurn = false;
        }
    }

    public void StartPlayerTurn()
    {
        if (_gm.turn == GameManager.Turn.Player)
        {
            isPlayerTurn = true;
        }
    }

    public void EndPlayerTurn()
    {
        //end using the a button to pass turn to the enemies
        if (_gm.turn == GameManager.Turn.Player)
        {
            _gm.turn = GameManager.Turn.Enemy;
            StartEnemyTurn();
            //run banner "Enemy's Turn"

        }
    }
    private void StartEnemyTurn()
    {
        if (_gm.turn == GameManager.Turn.Enemy)
        {
            enemyScript.ChooseNextAction();
            _gm.turn = GameManager.Turn.Player;
        }
    }
}
