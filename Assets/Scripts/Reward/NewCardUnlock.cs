using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCardUnlock : MonoBehaviour
{
    public Card chosenCard;
    public GameObject cardShow;

    public void ChooseCard()
    {
        chosenCard.currentStatus = Card.CardStatus.UNLOCKED;
        Debug.Log("Unlocking a new card: " + chosenCard.data.cardName + ", new status is: " + chosenCard.currentStatus);

        RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
        rewardSystem.rewardObjects.Remove(this.gameObject);

        GameManager gm = FindObjectOfType<GameManager>();
        gm.TurningOffReward();

        Destroy(this.gameObject);
    }

    public void ShowCard()
    {
        cardShow.SetActive(true);
        if (chosenCard != null)
        {
            CardUI ui = cardShow.GetComponent<CardUI>();
            ui.UpdateUI(chosenCard.data);
        }
    }

    public void HideCard()
    {
        cardShow.SetActive(false);
    }
}
