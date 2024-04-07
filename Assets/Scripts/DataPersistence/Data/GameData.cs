using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currencyCount;
    public SerializableDictionary<string, bool> collectibles;

    //the value defined in this constructor will be the default values
    //the game stats with when there's no data to load
    public GameData()
    {
        this.currencyCount = 0;
        collectibles = new SerializableDictionary<string, bool>();
    }
}


