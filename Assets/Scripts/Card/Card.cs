using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Sprite icon;
    public Sprite cardFront;
    public string cardName;

    public bool isStarter;

    public int stamCost;
    public string cardDes;
}
