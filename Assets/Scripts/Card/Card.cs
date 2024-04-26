using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Card Holder", order = 2)]
public class Card : ScriptableObject
{
    public CardData data;
}
