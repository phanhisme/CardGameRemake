using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI cardName;


    public void UpdateUI(CardData card)
    {
        image.sprite = card.icon;
        cardName.text = card.cardName;
    }
}
