using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class ChooseCharacter : ScriptableObject
{
    public Sprite icon;
    public Sprite splashArt;
    public string charName;

    public int maxHealth;

    //number of max turn depends on the characters
    public int energy;
    public int realmPower;
    public Realm realm;

    public enum Realm
    {
        DREAM, REALITY, NIGHTMARE
    }
}

