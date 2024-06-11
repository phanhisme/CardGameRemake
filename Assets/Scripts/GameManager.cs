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

    public Transform playerHand;
    public GameObject cardSlot;

    //player and turn
    [SerializeField] private GameObject player;
    [SerializeField] private EffectDuration effectDuration;

    public enum Turn { Player,Enemy};
    public Turn turn;

    public int energy; //number of max turn depends on the character
    public int realmPower; //power to use realm skills
    public TextMeshProUGUI energyText;

    //card
    public List<Card> starterDeck = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> discardedDeck = new List<Card>();

    //enemy
    public List<EnemyScriptableObject> enemyList = new List<EnemyScriptableObject>();
    public List<EnemyScriptableObject> enemyInStage = new List<EnemyScriptableObject>();
    public GameObject enemyObject;
    public Transform enemyHolder;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        effectDuration = player.GetComponent<EffectDuration>();

        SpawnEnemies();
    }
    void Update()
    {
        energyText.text = energy.ToString();
    }

    public void SpawnEnemies()
    {
        //chance to spawn double
        float spawnChance = Random.value;
        if (spawnChance < 0.3)
        {
            //spawn 2
            for (int i = 0; i < 2; i++)
            {
                //spawn object
                GameObject enemyToSpawn = Instantiate(enemyObject, enemyHolder);
                EnemyBehaviour eBehaviour = enemyToSpawn.GetComponentInChildren<EnemyBehaviour>();

                enemyInStage.Add(eBehaviour.enemyObject);
            }
        }
        else
        {
            GameObject enemyToSpawn = Instantiate(enemyObject, enemyHolder);
            EnemyBehaviour eBehaviour = enemyToSpawn.GetComponentInChildren<EnemyBehaviour>();

            enemyInStage.Add(eBehaviour.enemyObject);
        }
    }

    public void PlayerTurn()
    {
        if (selectedChar != null)
        {
            //player turn starts here
            turn = Turn.Player;
            energy = selectedChar.energy;


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

            //run player's turn baner
            turn = Turn.Player;
            energy = selectedChar.energy;

            //check buffs & debuffs
            Debug.Log(effectDuration);
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
        //else if (player){
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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
