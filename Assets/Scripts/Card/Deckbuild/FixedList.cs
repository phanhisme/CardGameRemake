using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FixedList : MonoBehaviour
{
    public Card card;

    public TextMeshProUGUI nameOfCard;
    public TextMeshProUGUI staminaNumber;
    public TextMeshProUGUI quantity;

    public void UpdateUI(Card card, int count)
    {
        nameOfCard.text = card.data.cardName;
        staminaNumber.text = card.data.stamCost.ToString();
        quantity.text = count.ToString();
    }

    public void RemoveCard()
    {
        Deckbuilding deck = FindObjectOfType<Deckbuilding>();
        GameManager gm = FindObjectOfType<GameManager>();

        if (deck.tempList.Contains(card))
        {
            deck.tempList.Remove(card);
        }
        else if (gm.starterDeck.Contains(card))
        {
            gm.starterDeck.Remove(card);
            deck.removed.Add(card);
        }

        foreach(GetCard getCard in deck.allCards) //get card number and decrease this
        {
            if (getCard.card == card)
            {
                getCard.RemoveCard();
            }
        }
    }
}
