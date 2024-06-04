using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCard : MonoBehaviour
{
    private Deckbuilding deck;
    public Card card;

    private void Start()
    {
        deck = FindObjectOfType<Deckbuilding>();

        CardUI cardUI = GetComponent<CardUI>();
        cardUI.UpdateUI(card.data);
    }

    public void AddCard()
    {
        deck.tempList.Add(card);
    }
}
