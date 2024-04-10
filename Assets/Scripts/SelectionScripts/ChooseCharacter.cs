using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class ChooseCharacter : ScriptableObject
{
    public Sprite icon;
    public Sprite splashArt;
    public string charName;

    //number of max turn depends on the characters
    public int energy;
}

