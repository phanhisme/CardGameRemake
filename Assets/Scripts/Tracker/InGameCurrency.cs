using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameCurrency : MonoBehaviour
{
    public int inGameCurrency = 0;
    public TextMeshProUGUI showCurrency;

    private void Update()
    {
        showCurrency.text = inGameCurrency.ToString();
    }
}
