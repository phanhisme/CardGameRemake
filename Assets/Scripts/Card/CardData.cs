using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    //public GameObject cardPrefab;
    
    public Sprite icon;
    public Sprite cardFront;
    public string cardName;

    public int stamCost;
    public int effectAmount;

    public string cardDes;
    public CardType cardType;

    public enum CardType
    {
        Attack, Defense, Buff, Debuff
    }
}
