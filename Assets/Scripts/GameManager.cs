using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //script reference
    public OnDeckBehaviour selectedCard;
    public ChooseCharacter selectedChar;
    private CharacterSelection.Character character;

    //floorManager
    public TextMeshProUGUI floorText;
    [SerializeField] private int currentFloor;

    //card
    public Transform playerHand;
    public GameObject cardSlot;

    //player and turn
    [SerializeField] private GameObject player;
    [SerializeField] private EffectDuration effectDuration;
    [SerializeField] private BasePlayer basePlayer;

    public enum Turn { Player,Enemy};
    public Turn turn; 

    //card
    public List<Card> starterDeck = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> discardedDeck = new List<Card>();

    //enemy
    public List<EnemyScriptableObject> enemyList = new List<EnemyScriptableObject>();
    public List<EnemyScriptableObject> enemyInStage = new List<EnemyScriptableObject>();
    public GameObject enemyObject;
    public Transform enemyHolder;

    //animation
    public Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        effectDuration = player.GetComponent<EffectDuration>();
        basePlayer = player.GetComponent<BasePlayer>();

        currentFloor = 1;
        floorText.text = "Floor " + currentFloor;

        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (currentFloor < 3)
        {
            //guarantee to spawn 1 enemy
            AddEnemies();
        }
        else if (currentFloor == 6 || currentFloor == 12)
        {
            //meet shop
        }
        else if (currentFloor == 15)
        {
            //meet restRoom
        }
        else
        {
            //chance to spawn double
            float spawnChance = Random.value;
            if (spawnChance < 0.3)
            {
                //spawn 2
                for (int i = 0; i < 2; i++)
                {
                    //spawn object
                    AddEnemies();
                }
            }
            else
            {
                //guarantee to spawn 1 enemy
                AddEnemies();
            }
        }
    }

    public void AddEnemies()
    {
        GameObject enemyToSpawn = Instantiate(enemyObject, enemyHolder);
        EnemyBehaviour eBehaviour = enemyToSpawn.GetComponentInChildren<EnemyBehaviour>();
        enemyInStage.Add(eBehaviour.enemyObject);
    }

    public void CheckEnemies()
    {
        if (enemyInStage.Count == 0)
        {
            Debug.Log("All enemies are defeated, reset to new floor");
            currentFloor++;

            //reset deck
            if (playerDeck.Count != 0)
            {
                foreach(Card card in playerDeck)
                {
                    starterDeck.Add(card);
                    playerDeck.Remove(card);
                }
            }

            if (discardedDeck.Count != 0)
            {
                foreach (Card card in discardedDeck)
                {
                    starterDeck.Add(card);
                    discardedDeck.Remove(card);
                }
            }

            //reset player
            basePlayer.ResetToStart();

            anim.SetTrigger("NewRound");
            StartCoroutine(WaitForSeconds(1f));
        }
    }

    public void PlayerTurn()
    {
        if (selectedChar != null)
        {
            //player turn starts here
            turn = Turn.Player;
            basePlayer.ResetEnergy();

            //shuffle deck and give out 5 starter card
            ShuffleDeck(5);

            //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7
        }
    }
    
    public void ShuffleDeck(int numCards)
    {
        //shuffle deck and give out 5 starter card
        //player can only hold 7 cards at max -> discard if the the number of card on hand get > 7

        for (int i = 0; i < numCards; i++)
        {
            int x = Random.Range(0, starterDeck.Count);

            //get data from scriptable object
            Card card = starterDeck[x];

            CardData cardData = card.data;

            GameObject thisCard = Instantiate(cardSlot, playerHand);

            //display it on UI
            thisCard.GetComponent<CardUI>().UpdateUI(cardData);
            
            //find scriptable
            OnDeckBehaviour onDeck = thisCard.GetComponent<OnDeckBehaviour>();
            onDeck.SetCardData(cardData);
            onDeck.FindCard(card);

            playerDeck.Add(card);
            starterDeck.Remove(card);
        }
    }

    public void CheckCardOnHand()
    {
        if (playerDeck.Count > 7)
        {
            //DISCARD
            //Open new panel
        }
    }

    public void EndPlayerTurn()
    {
        Debug.Log("End of " + turn);
        //end using the a button to pass turn to the enemies
        if (turn == Turn.Player)
        {
            //CheckForRelic(character);
            if (character == CharacterSelection.Character.Dabria)
            {
                DabriaStarterRelic relic = FindObjectOfType<DabriaStarterRelic>();
                //effect after fight, not turn
                //relic.DabRelicEffect();
            }

            //if keep the block for next turn
            CheckBlockStatus();

            turn = Turn.Enemy;
            EnemyTurn();
            //run banner "Enemy's Turn"

            //end of currentTurn - current turn = start status at next turn

            EffectDuration effectDuration = FindObjectOfType<EffectDuration>();
            effectDuration.RemoveTurn();

        }
    }
    private void EnemyTurn()
    {
        if (turn == Turn.Enemy)
        {
            //StartCoroutine(WaitForSeconds(0.5f));
            GameObject[] enemyL = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject entity in enemyL)
            {
                EnemyBehaviour e = entity.GetComponent<EnemyBehaviour>();
                e.ChooseNextAction();
                StartCoroutine(WaitForSeconds(0.5f));
            }

            StartCoroutine(WaitForSeconds(0.5f));

            //run player's turn baner
            turn = Turn.Player;

            //the scripts are null?
            basePlayer = FindObjectOfType<BasePlayer>();
            effectDuration = FindObjectOfType<EffectDuration>();

            basePlayer.ResetEnergy();

            //check buffs & debuffs
            effectDuration.CheckLullaby();
            effectDuration.CheckToxic();
        }
    }

    public void EndFight()
    {
        if (enemyList == null)
        {
            //all enemies are dead
            //Banner fight end
        }
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        Debug.Log("Paused");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
        Debug.Log("Done Pause");
    }

    public int GetDefenseCard()
    {
        int i = 0;
        foreach (Card cards in playerDeck)
        {
            if (cards.data.cardType == CardData.CardType.Defense)
            {
                i++;
            }
        }

        Debug.Log("The number of defensive cards are: " + i);
        return i;
    }

    public void CheckBlockStatus()
    {
        EffectDuration effectManager = FindObjectOfType<EffectDuration>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        BasePlayer playerScript = player.GetComponent<BasePlayer>();

        if (!effectManager.appliedStatus.Contains(effectManager.allStatus[7]))
        {
            Debug.Log("Removing blocks");
            playerScript.RemoveBlock();
        }
        else
            Debug.Log("Do not remove block");
    }

    public void ResetDeck(Deckbuilding deck)
    {
        foreach (Card card in deck.tempList)
        {
            starterDeck.Add(card);
        }

        deck.tempList.Clear();
        deck.removed.Clear();
    }
}
