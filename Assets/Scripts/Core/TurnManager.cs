using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameManager _gm;
    private EnemyBehaviour enemyScript;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
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
            
        }
    }

}
