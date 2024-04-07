using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectiblesDisplay : MonoBehaviour, IDataPersistence
{
    private int totalCollectibles = 3;
    public int currentCollectibles = 0;

    public TextMeshProUGUI colText;

    void Start()
    {
        
    }

    private void Update()
    {
        colText.text = currentCollectibles + " / " + totalCollectibles;
    }

    public void LoadData(GameData data)
    {
        foreach(KeyValuePair<string,bool> pair in data.collectibles)
        {
            if (pair.Value)
            {
                currentCollectibles++;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        //no data needs to be saved for this script
    }
}
