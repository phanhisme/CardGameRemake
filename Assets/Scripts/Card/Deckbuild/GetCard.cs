using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCard : MonoBehaviour
{
    private Deckbuilding deck;
    private GameManager gm;

    public List<GameObject> indCount = new List<GameObject>();
    public GameObject indObject;
    public GameObject lockPanel;
    public Sprite indicator;
    public Sprite e_indicator; //emptyIndicator
    public Transform _holder;

    public Card card;
    public int initialCardNumber;
    public int cardNumber;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        deck = FindObjectOfType<Deckbuilding>();

        CardUI cardUI = GetComponent<CardUI>();
        cardUI.UpdateUI(card.data);

        initialCardNumber = card.maxNumber;

        SetUpIndicator(initialCardNumber);

        cardNumber = CheckCardNumber();
        ChangeSpriteIndicator();

        if (card.currentStatus == Card.CardStatus.UNLOCKED)
        {
            lockPanel.SetActive(false);
        }
    }

    public int CheckCardNumber()
    {
        int cardUsed = 0;

        foreach (Card gmCard in gm.starterDeck)
        {
            if (gmCard.data.ID == card.data.ID)
            {
                cardUsed++;
            }
        }
        return cardUsed;
    }

    private void SetUpIndicator(int maxInt)
    {
        if (indCount.Count == 0)
        {
            //set up first
            for (int i = 0; i < maxInt; i++)
            {
                GameObject e_ind = Instantiate(indObject, _holder);
                indCount.Add(e_ind);
            }
        }
    }

    private void ChangeSpriteIndicator()
    {
        //int count = usedInt; //number of cards that have changed

        if (indCount.Count > 0)
        {
            int counter = 0; //stops the sprite change at the card number
            int fixedNumber = initialCardNumber - CheckCardNumber(); //get the number of empty indicator

            foreach (GameObject gameObject in indCount)
            {
                Image image = gameObject.GetComponent<Image>();

                if (counter < cardNumber) //counter starts with 0, while cardNumber starts with 1
                {
                    image.sprite = indicator;
                    counter++;
                }

                else if (fixedNumber >= counter)
                {
                    image.sprite = e_indicator; //change sprite to empty indicator
                    fixedNumber--; //stops
                }
            }
        }
    }

    public void AddCard()
    {
        if (card.currentStatus == Card.CardStatus.UNLOCKED) //only when the cards are unlocked that can be added in the deck
        {
            //only add if the current number is < initial number
            if (cardNumber < initialCardNumber)
            {
                deck.tempList.Add(card);

                deck.TemporarySetUp();
                deck.AddItem(card);

                cardNumber += 1;
                ChangeSpriteIndicator();
            }
        }
    }

    public void RemoveCard()
    {
        if (cardNumber > 0)
        {
            //deck.tempList.Remove(card);
            deck.MinusItem(card);

            cardNumber -= 1;
            ChangeSpriteIndicator();
        }
    }

    public void ClearCard()
    {
        cardNumber = CheckCardNumber();
        ChangeSpriteIndicator();
    }
}
