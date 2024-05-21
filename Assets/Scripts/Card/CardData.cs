using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Card Data", order = 1)]
public class CardData : ScriptableObject
{
    //public GameObject cardPrefab;

    public int ID;

    public Sprite icon;
    public Sprite cardFront;
    public string cardName;

    public int stamCost;
    public int effectAmount;
    public int duration; //turn

    public string cardDes;
    public CardType cardType;

    public enum CardType
    {
        Attack, Defense, Skill
    }
}
