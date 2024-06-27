using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DabriaStarterRelic : MonoBehaviour
{
    //this is Dabria's specialty starter relic - tiny hope
    //this script is disabled by default and will only be acessible if the player choose the character "Dabria"

    //at the start of a new round, Dabria will heal 2HP. After 4 rounds and above 50HP, Dabria will receive a "blessing"
    //types of blessings: coin (ig currency), HP, extra card to deck, extra card to deck on hand, turn, random relic...

    public int blessingCount = 0;

    public enum Blessings { Health, Currencies, ExtraCard}
    private Blessings randomBlessing;

    private BasePlayer player;

    public void RelicUIUpdate()
    {
        RelicDisplay relicDisplay = FindObjectOfType<RelicDisplay>();
        relicDisplay.DabRelic(this);
    }

    public void DabRelicEffect()
    {
        blessingCount += 1;
        RelicUIUpdate();

        player = FindObjectOfType<BasePlayer>();
        if (player != null)
        {
            if (blessingCount == 4)
            {
                if (player.HealthCheck())
                {
                    Debug.Log("You received Blessings from the Moon");

                    int randBlessing = Random.Range(0, 3); //I CANT USE BLESSING.LENGHT, WHY?
                    switch (randBlessing)
                    {
                        case 0:
                            randomBlessing = Blessings.Health;
                            Debug.Log("You receive 1 health");
                            player.HealUp(1);

                            break;

                        case 1:
                            randomBlessing = Blessings.Currencies;
                            Debug.Log("You receive 10 currency");

                            InGameCurrency currency = FindObjectOfType<InGameCurrency>();
                            currency.inGameCurrency += 10;

                            break;

                        case 2:
                            randomBlessing = Blessings.ExtraCard;
                            Debug.Log("You receive an extra card");

                            GameManager gm = FindObjectOfType<GameManager>();
                            gm.ShuffleDeck(1);
                            break;

                        case 3:
                            player.energy += 1;
                            Debug.Log("You receive 1 Energy");
                            break;
                    }
                }

                blessingCount = 0;
                RelicUIUpdate();
            }
        }
    }
}
