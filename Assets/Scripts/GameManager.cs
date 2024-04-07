using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int maxTurn; //number of max turn depends on the character
    private int currentTurn; //number of current turn

    private bool atMaxTurn;

    void Start()
    {
        //player starts turn first
        PlayerTurn();
    }

    void Update()
    {
        if (currentTurn == maxTurn)
        {
            atMaxTurn = true;
        }
    }

    public void PlayerTurn()
    {
        //player move 
        //per-turn heal up if acquired healing relic

        currentTurn = maxTurn;

        //shuffle deck and give out 5 starter card
        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        
    }

    public void EndPlayerTurn()
    {
        //end using the 
    }
}
