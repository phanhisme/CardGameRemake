using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image image;
    private TextMeshProUGUI cardName;

    public void UpdateUI(Card card)
    {
        image.sprite = card.icon;
        cardName.text = card.cardName;
    }
}
