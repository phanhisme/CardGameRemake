using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardItem
{
    [SerializeField] public Card card;
    [SerializeField] public int cardCount;

    public CardItem(Card cardScriptable, int count)
    {
        card = cardScriptable;
        cardCount = count;
    }

    public Card GetItems() { return card; }
    public int GetInventoryNumber() { return cardCount; }
    public void AddQuantity(int _quantity) { cardCount += _quantity; }
    public void SubQuantity(int _quantity) { cardCount -= _quantity; }
}
