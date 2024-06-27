using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyToAdd : MonoBehaviour
{
    public int currencyToAdd;

    //record the currency to add of the reward
    //there might be multiple drops of currency in the future which can confuse the main system script if not handled carefully
    //this is a script to record the correct currency to avoid that scenario

    public void ChooseCurrency()
    {
        InGameCurrency currency = FindObjectOfType<InGameCurrency>();
        currency.inGameCurrency += currencyToAdd;

        RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
        rewardSystem.rewardObjects.Remove(this.gameObject);

        GameManager gm = FindObjectOfType<GameManager>();
        gm.TurningOffReward();

        Destroy(this.gameObject);
    }
}
