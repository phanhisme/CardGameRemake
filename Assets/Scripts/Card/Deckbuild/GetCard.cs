using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCard : MonoBehaviour
{
    private Deckbuilding deck;
    private GameManager gm;

    public GameObject indicator;
    public GameObject r_indicator;
    public Transform _holder;

    public Card card;
    public int initialCardNumber;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        deck = FindObjectOfType<Deckbuilding>();

        CardUI cardUI = GetComponent<CardUI>();
        cardUI.UpdateUI(card.data);

        initialCardNumber = card.maxNumber;

        SetUpIndicator(initialCardNumber, CheckCardNumber());
    }

    private int CheckCardNumber()
    {
        int cardUsed = 0;

        foreach (Card gmCard in gm.starterDeck)
        {
            if (gmCard.data.ID == card.data.ID)
            {
                cardUsed++;
            }
        }
        return cardUsed;
    }

    private void SetUpIndicator(int maxInt, int usedInt)
    {
        //instantiate the used item first them max int-used= item to spawn next
        for (int i = 0; i < usedInt; i++)
        {
            GameObject r_ind = Instantiate(r_indicator, _holder);
        }

        for(int n = 0; n < (maxInt - usedInt); n++)
        {
            GameObject ind = Instantiate(indicator, _holder);
        }
    }

    public void AddCard()
    {
        deck.tempList.Add(card);
        initialCardNumber--;

        CheckCardNumber();
    }
}
