using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Confirm_Decline : MonoBehaviour
{
    private Deckbuilding deck;
    private GameManager gm;

    private void Start()
    {
        deck = FindObjectOfType<Deckbuilding>();
        gm = FindObjectOfType<GameManager>();
    }

    public void ClearAllCards()
    {
        foreach(Card removedCards in deck.removed) //restore the cards back to the deck it was removed from
        {
            gm.starterDeck.Add(removedCards);
            foreach (GetCard getCard in deck.allCards)
            {
                if (getCard.card == removedCards)
                {
                    getCard.ClearCard();
                }

                deck.SetUpItem(); //UI update
            }
        }

        foreach(Card card in deck.tempList)
        {
            foreach (GetCard getCard in deck.allCards)
            {
                if (getCard.card == card)
                {
                    getCard.ClearCard();
                }
            }

            deck.MinusItem(card); //return the inventoryNumber back to the correct number
        }

        deck.removed.Clear(); //clear temp lists
        deck.tempList.Clear();
    }

    public void ConfirmDeck()
    {
        //change scene
        //SceneManager.LoadScene("01_Gameplay", LoadSceneMode.Additive);

        gm.ResetDeck(deck); //reset
        deck.SetUpItem(); //set up UI
    }
}
