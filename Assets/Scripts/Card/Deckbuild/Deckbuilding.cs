using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilding : MonoBehaviour
{
    public List<GetCard> allCards = new List<GetCard>();
    
    public List<Card> tempList = new List<Card>();
    public List<Card> removed = new List<Card>();
    public List<CardItem> cardShown = new List<CardItem>();
    private List<FixedList> fixedList = new List<FixedList>();

    public GameObject fixedItem;
    public Transform _holder;

    private GameManager gm;

    //NOTE: First 15 cards will automatically add into the deck while after that the card will be left unchecked regardless of the status.
    //The player can have 15 cards per deck.

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void SetUpItem()
    {
        //setting initial items
        
        foreach (Card card in gm.starterDeck)
        {
            if (!cardShown.Contains(FindCorrectScriptable(card))) //creating new CardItem and its inventory number
            {
                int count = GetInitialNumber(card);

                cardShown.Add(new CardItem(card, count));
                GameObject newItem = Instantiate(fixedItem, _holder);

                FixedList dataScript = newItem.GetComponent<FixedList>();
                fixedList.Add(dataScript);
                dataScript.card = card;
                dataScript.UpdateUI(card, count);
            }

            else
            {
                UpdateUI(FindCorrectScriptable(card), card, FindObject(card)); //if CardItem already exist, update UI
            } 
        }
    }

    public void TemporarySetUp()
    {
        foreach (Card card in tempList) //similar but do not stay permanent
        {
            if (!cardShown.Contains(FindCorrectScriptable(card)))
            {
                int count = GetInitialNumber(card);

                cardShown.Add(new CardItem(card, count));
                GameObject newItem = Instantiate(fixedItem, _holder);

                FixedList dataScript = newItem.GetComponent<FixedList>();
                fixedList.Add(dataScript);
                dataScript.card = card;
                dataScript.UpdateUI(card, count);
            }

            else
            {
                UpdateUI(FindCorrectScriptable(card), card, FindObject(card));
            }
        }
    }

    public void AddItem(Card card)
    {
        CardItem cardItem = FindCorrectScriptable(card);
        cardItem.AddQuantity(1);

        UpdateUI(FindCorrectScriptable(card), card, FindObject(card));
    }

    public void MinusItem(Card card)
    {
        CardItem cardItem = FindCorrectScriptable(card);
        cardItem.SubQuantity(1);

        if (cardItem.cardCount <= 0)
        {
            cardShown.Remove(cardItem);

            FixedList data = FindObject(card);
            Destroy(data.gameObject);
            fixedList.Remove(data);
        }
        else
            UpdateUI(FindCorrectScriptable(card), card, FindObject(card));
    }

    public FixedList FindObject(Card card)
    {
        foreach(FixedList dataScript in fixedList)
        {
            if (dataScript.card == card)
            {
                return dataScript;
            }
        }

        return null;
    }

    private void UpdateUI(CardItem cardItem, Card card, FixedList data)
    {
        cardItem = FindCorrectScriptable(card);
        data.UpdateUI(card, cardItem.GetInventoryNumber());
    }

    private CardItem FindCorrectScriptable(Card card)
    {
        foreach (CardItem i in cardShown)
        {
            if (i.GetItems() == card)
                return i;
        }

        return null;
    }

    private int GetInitialNumber(Card thisCard) //number of card at the start (after interacting with the ui
    {
        int cardNumber = 0;
        gm = FindObjectOfType<GameManager>();
        foreach (Card gmCard in gm.starterDeck)
        {
            if (gmCard.data.ID == thisCard.data.ID)
            {
                cardNumber++;
            }
        }
        return cardNumber;
    }
}
