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
    public GameObject cardSlot;

    //player and turn
    public enum Turn { Player,Enemy};
    public Turn turn;

    public enum Character { Dabria,Daella,Asif,Amias,Maeve };
    public Character character;

    public int energy; //number of max turn depends on the character
    public TextMeshProUGUI energyText;

    //card
    public List<Card> starterDeck = new List<Card>();
    public List<GameObject> playerDeck = new List<GameObject>();


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
        //player turn starts here
        turn = Turn.Player;

        //shuffle deck and give out 5 starter card
        ShuffleDeck(5);

        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7
    }
    
    private void ShuffleDeck(int numCards)
    {
        //shuffle deck and give out 5 starter card
        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        for (int i = 0; i < numCards; i++)
        {
            int x = Random.Range(0, starterDeck.Count);

            //get data from scriptable object
            Card cardScriptable = starterDeck[x];

            CardData cardData = cardScriptable.data;

            GameObject thisCard = Instantiate(cardSlot, playerHand);

            //display it on UI
            thisCard.GetComponent<CardUI>().UpdateUI(cardData);
            
            playerDeck.Add(thisCard);
        }
    }
}
