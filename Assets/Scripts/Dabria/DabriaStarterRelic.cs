using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DabriaStarterRelic : MonoBehaviour
{
    //this is Dabria's specialty starter relic - tiny hope
    //this script is disabled by default and will only be acessible if the player choose the character "Dabria"

    //at the start of turn, Dabria will heal 2HP. After 4 rounds and above 50HP, Dabria will receive a "blessing"
    //types of blessings: coin (ig currency), HP, extra card to deck, extra card to deck on hand, turn, random relic...

    public string relicName;
    public Image relicIcon;

    private bool conditionSatisfy;
    private int blessingCount = 0;

    public List<DabriaBlessings> blessings = new List<DabriaBlessings>();
    public DabriaBlessings claimedBlessings;

    //display
    public TextMeshProUGUI relicNameText;
    public TextMeshProUGUI blessingCountText;

    private void Start()
    {
        relicNameText.text = relicName;
        blessingCountText.text = blessingCount.ToString();
        
    }
    public void DabRelicActivated()
    {
        if (conditionSatisfy)
        {
            //dabria.currentHP += 2;
            //buffCount++;
        }
    }

    public void Update()
    {
        //if (buffCount == 4)
        //{
        //    buffCount = 0;
            
        //    //pull a random blessing
        //    int x = Random.Range(0, blessings.Count);
        //}
    }

}
