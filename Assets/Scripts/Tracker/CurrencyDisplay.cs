using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour, IDataPersistence
{
    private int currencyCount = 0;
    public TextMeshProUGUI currencyText;

    void Start()
    {
        //currencyText = this.GetComponent<TextMeshProUGUI>();
    }

    public void LoadData(GameData data)
    {
        this.currencyCount = data.currencyCount;
    }

    public void SaveData(ref GameData data)
    {
        data.currencyCount = this.currencyCount;
    }

    public void AddCurrency()
    {
        currencyCount++;
        //+1 for now
    }

    void Update()
    {
        currencyText.text = " " + currencyCount;
    }
}
