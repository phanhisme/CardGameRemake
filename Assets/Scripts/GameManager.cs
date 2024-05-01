using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //script reference
    public OnDeckBehaviour selectedCard;

    public ChooseCharacter selectedChar;
    private CharacterSelection.Character character;

    public Transform playerHand;
    public GameObject cardSlot;
    public Button endTurnButton;

    //player and turn
    public enum Turn { Player,Enemy};
    public Turn turn;

    public int energy; //number of max turn depends on the character
    public TextMeshProUGUI energyText;

    //card
    public List<Card> starterDeck = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> discardedDeck = new List<Card>();

    void Start()
    {
        //selected char will be selected in another scene and bring over to this scene in a dont destroy script
        //for now we will use public char {dabria} for testing

        //player starts turn first

        
    }

    void Update()
    {
        energyText.text = "" + energy.ToString();
    }

    public void PlayerTurn()
    {
        if (selectedChar != null)
        {
            //player turn starts here
            turn = Turn.Player;
            energy = selectedChar.energy;


            //shuffle deck and give out 5 starter card
            ShuffleDeck(5);

            //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7
        }
    }
    
    public void ShuffleDeck(int numCards)
    {
        //shuffle deck and give out 5 starter card
        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        for (int i = 0; i < numCards; i++)
        {
            int x = Random.Range(0, starterDeck.Count);

            //get data from scriptable object
            Card card = starterDeck[x];

            CardData cardData = card.data;


            GameObject thisCard = Instantiate(cardSlot, playerHand);

            //display it on UI
            thisCard.GetComponent<CardUI>().UpdateUI(cardData);
            
            //find scriptable
            OnDeckBehaviour onDeck = thisCard.GetComponent<OnDeckBehaviour>();
            onDeck.SetCardData(cardData);
            
            playerDeck.Add(card);
        }
    }

    public void StartPlayerTurn()
    {
        if (turn == Turn.Player)
        {

        }
    }

    public void EndPlayerTurn()
    {
        //end using the a button to pass turn to the enemies
        if (turn == Turn.Player)
        {
            endTurnButton.enabled = false;

            //check for dabria relic (happens when turn ends)
            if (character == CharacterSelection.Character.Dabria)
            {
                DabriaStarterRelic relic = FindObjectOfType<DabriaStarterRelic>();
                relic.DabRelicEffect();
            }

            //check for the remaining cards and add to the discard deck
            foreach (Card card in playerDeck)
            {
                Debug.Log(playerDeck.Count);
                DiscardCard(card);
            }

            //foreach (CardUI)

            turn = Turn.Enemy;
            EnemyTurn();
            //run banner "Enemy's Turn"

        }
    }
    private void EnemyTurn()
    {
        if (turn == Turn.Enemy)
        {
            StartCoroutine(WaitForSeconds(0.5f));
            EnemyBehaviour enemyScript = FindObjectOfType<EnemyBehaviour>();
            enemyScript.ChooseNextAction();

            //run player's turn baner
            StartCoroutine(WaitForSeconds(0.5f));
            turn = Turn.Player;
            energy = selectedChar.energy;
        }
    }

    public void DiscardCard(Card card)
    {
        //THIS IS NOT WORKING!

        //discardedDeck.AddRange(playerDeck);
        //
        //playerDeck.Clear();
        //

        discardedDeck.Add(card);
        Debug.Log(discardedDeck.Count);

        //update number of card in discarded deck here
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}