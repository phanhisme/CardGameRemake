using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image borderType;
    public Image icon;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDes;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI cost;


    public void UpdateUI(CardData card)
    {
        icon.sprite = card.icon;
        borderType.sprite = card.cardFront;

        cardName.text = card.cardName;
        cardDes.text = card.cardDes;

        cardType.text = card.cardType.ToString();
        cost.text = card.stamCost.ToString();
    }
}
