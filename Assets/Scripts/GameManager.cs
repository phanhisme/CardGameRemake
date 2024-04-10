using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //script reference
    public OnDeckBehaviour selectedCard;
    public ChooseCharacter selectedChar;
    public Transform playerHand;

    //player and turn
    public enum Turn { Player,Enemy};
    public Turn turn;

    public enum Character { Dabria,Daella,Asif,Amias,Maeve };
    public Character character;

    public int energy; //number of max turn depends on the character
    public TextMeshProUGUI energyText;

    //card
    public List<Card> starterDeck = new List<Card>();
    public List<Card> playerDeck = new List<Card>();


    void Start()
    {
        //selected char will be selected in another scene and bring over to this scene in a dont destroy script
        //for now we will use public char {dabria} for testing

        //player starts turn first

        if (selectedChar != null)
        {
            Debug.Log("You have chosen " + selectedChar.charName);
            energy = selectedChar.energy;
            PlayerTurn();
        }
    }

    void Update()
    {
        energyText.text = "" + energy.ToString();
    }

    public void PlayerTurn()
    {
        //player move 
        turn = Turn.Player;
        Debug.Log("Turn " + turn);

        //shuffle deck and give out 5 starter card
        ShuffleDeck();

        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        
    }

    public void EndPlayerTurn()
    {
        //end using the a button to pass turn to the enemies

    }
    
    private void ShuffleDeck()
    {
        //shuffle deck and give out 5 starter card
        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        for (int i = 0; i < 7; i++)
        {
            int x = Random.Range(1, starterDeck.Count);
            //Debug.Log("Card" + x);
            playerDeck.Add(starterDeck[x]);

            Instantiate(starterDeck[x], playerHand); //not working at the moment
        }
    }

}
