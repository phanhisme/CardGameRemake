using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "Card/Status", order = 3)]
public class Status : ScriptableObject
{
    public string statusID;

    public string effectName;
    public Sprite effectIcon;

    public string effectDescription;

    public int effectDuration;
    public StatusType effectType;

    public enum StatusType
    {
        Buff, Debuff
    }
}
