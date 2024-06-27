using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardSystem : MonoBehaviour
{
    public List<GameObject> rewardObjects = new List<GameObject>();

    public Transform rewardTransform;
    public GameObject rewardCurrency;
    public GameObject rewardCardItem;

    public void GiveReward()
    {
        GameObject currencyReward = Instantiate(rewardCurrency, rewardTransform); //this drop is guarantee
        rewardObjects.Add(currencyReward);

        GameManager gm = FindObjectOfType<GameManager>();
        int floorNumber = gm.returnFloor();

        CurrencyToAdd toAdd = currencyReward.GetComponent<CurrencyToAdd>(); //floor completion rewards
        toAdd.currencyToAdd = getSoulFragmentDrop();

        TextMeshProUGUI currencyText = currencyReward.GetComponentInChildren<TextMeshProUGUI>();
        currencyText.text = toAdd.currencyToAdd + " Soul Fragments";

        float chancesToObtain = Random.value; //chances to obtain this drop

        if (floorNumber < 5)
        {
            if (chancesToObtain < 0.5f) //50%
            {
                InstantiateCardReward(GetAvailCard());
            }
        }
        else if (floorNumber < 10)
        {
            if (chancesToObtain < 0.7f) //70%
            {
                InstantiateCardReward(GetAvailCard());
            }
        }
        else
        {
            if (chancesToObtain < 1f) //100%
            {
                InstantiateCardReward(GetAvailCard());
            }
        }
    }

    public int getSoulFragmentDrop()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        int soulFragmentDrop = 0;
        int floorNumber = gm.returnFloor();

        if (floorNumber < 5)
        {
            return soulFragmentDrop = Random.Range(100, 150);
        }
        else if(floorNumber < 10)
        {
            return soulFragmentDrop = Random.Range(200, 250);
        }
        else
            return soulFragmentDrop = Random.Range(350, 500);
    }


    public void InstantiateCardReward(Card availCard)
    {
        GameObject newCardItem = Instantiate(rewardCardItem, rewardTransform);
        rewardObjects.Add(newCardItem);

        NewCardUnlock newCard = newCardItem.GetComponent<NewCardUnlock>();
        newCard.chosenCard = availCard;
    }

    public Card GetAvailCard()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        List<Card> lockedCards = new List<Card>();

        foreach (Card card in gm.allAvailableCards)
        {
            if (card.currentStatus == Card.CardStatus.LOCKED)
            {
                lockedCards.Add(card);
            }
        }

        if (lockedCards.Count > 0)
        {
            int rand = Random.Range(0, lockedCards.Count);
            return lockedCards[rand];
        }
        else
            return null;    
    }

    public int returnItemNumber()
    {
        return rewardObjects.Count;
    }
}
