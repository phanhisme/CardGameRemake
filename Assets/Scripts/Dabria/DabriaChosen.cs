using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DabriaChosen : BasePlayer
{
    private CharacterSelection charScript;
    public DabriaStarterRelic dabRelic;

    private GameManager gm;

    public List<Card> starterDreamDeck = new List<Card>();
    public List<Card> dreamDeck = new List<Card>();
    public List<Card> playerDeck = new List<Card>();

    private int dabriaDeckTurn;

    private bool turnAvail = false;

    public int maxHP;
    public int currentHP;
    

    private void Start()
    {
        charScript = FindObjectOfType<CharacterSelection>();
        //dabRelic.GetComponent<DabriaStarterRelic>();
        //gm = FindObjectOfType<GameManager>();

        int i = 1;
        if (i == 1) //dabria is chosen
        {
            //continue
            //recieve relic - enable a passive skill through script

            dabRelic.enabled = true;
            //gm.PlayerTurn();

            StarterRun();
        }
    }

    private void StarterRun()
    {
        //shuffle deck and give out 5 starter card
        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        for( int i = 0; i < 5; i++)
        {
            int x = Random.Range(1, starterDreamDeck.Count);
            Debug.Log("Card" + x);
            playerDeck.Add(starterDreamDeck[x]);
        }

        //receive 5 cards
        //calculate turns

        dabriaDeckTurn = 3;
    }

    private void Update()
    {
        if (dabriaDeckTurn > 0 && turnAvail)
        {
            //dabria can take turns
            //condition satisfaction comes with deck turn (depends on the number of turn left)

            StartTurn();
            //this do not consider the first turn!!!!!!!!
        }
        else
        {
            //dabria cannot take turn -> forced to end turn

            EndTurn();
        }

        //at start of turn, dabria receives 3 turn (as well)
    }

    private void StartTurn()
    {
        //after making a move, -1 turn from poll
        dabriaDeckTurn = 3;

    }

    public void EndTurn()
    {
        //move turn to enemy
        StartCoroutine(PauseGame());
    }

    IEnumerator PauseGame()
    {
        Debug.Log("Changing turn to enemies");
        yield return new WaitUntil(() => turnAvail); //wait until turn avail to true
        Debug.Log("Changing back to player turn");
    }
}

