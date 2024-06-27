using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

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

    public enum Turn { Player, Enemy };
    public Turn turn;

    //card
    public GameObject cardMenu;
    public List<Card> allAvailableCards = new List<Card>();
    public List<Card> starterDeck = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> discardedDeck = new List<Card>();

    //enemy
    public List<EnemyScriptableObject> enemyList = new List<EnemyScriptableObject>();
    public List<EnemyScriptableObject> enemyInStage = new List<EnemyScriptableObject>();
    public GameObject enemyObject;
    public Transform enemyHolder;

    //animation
    //public Animator anim;

    public enum DeckStatus { READY, BUSY } //ready means, can change while busy mean cannot change the card set up
    public DeckStatus deckStatus = DeckStatus.READY;

    int floorNumber = 1;
    private RewardSystem rewardSystem;
    public GameObject rewardPanel;
    public GameObject selectorPanel;
    public GameObject pausePanel;

    public TextMeshProUGUI starterDeckText;
    public TextMeshProUGUI discardedDeckText;

    public GameObject markOfRebirth;

    private void Start()
    {
        rewardPanel.SetActive(false);
        selectorPanel.SetActive(true);

        player = GameObject.FindGameObjectWithTag("Player");
        effectDuration = player.GetComponent<EffectDuration>();
        basePlayer = player.GetComponent<BasePlayer>();
        rewardSystem = FindObjectOfType<RewardSystem>();
        currentFloor = 1;

        SpawnEnemies();
    }

    private void Update()
    {
        floorText.text = "Floor " + currentFloor;
        starterDeckText.text = starterDeck.Count.ToString();
        discardedDeckText.text = discardedDeck.Count.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

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

        enemyInStage.Add(eBehaviour.ChooseEnemies());
        eBehaviour.enemyObject = enemyInStage[enemyInStage.Count - 1];
    }

    public void CheckEnemies()
    {
        if (enemyInStage.Count == 0)
        {
            Debug.Log("All enemies are defeated, reset to new floor");

            if (discardedDeck.Count != 0)
            {
                for (int i = discardedDeck.Count - 1; i >= 0; i--)
                {
                    Card card = discardedDeck[i];
                    starterDeck.Add(card);
                    discardedDeck.RemoveAt(i);
                }
            }

            FinishFight();

            //reset player
            basePlayer.ResetToStart();

            //anim.SetTrigger("NewRound");
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

        int cardsToDraw = Mathf.Min(numCards, starterDeck.Count);

        for (int i = 0; i < cardsToDraw; i++)
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

            //if (i == starterDeck.Count)
            //{
            //    break;
            //}
        }

        //if there is no more card to pull, take cards from the discarded deck
        MoveCardToPullDeck();
    }

    public void CheckCardOnHand()
    {
        if (playerDeck.Count == 0)
        {
            ShuffleDeck(5);
        }

        if (playerDeck.Count > 7)
        {
            //DISCARD
            //Open new panel
        }
    }

    public void EndPlayerTurn()
    {
        //Debug.Log("End of " + turn);
        //end using the a button to pass turn to the enemies
        if (turn == Turn.Player)
        {
            //CheckForRelic(character);
            if (character == CharacterSelection.Character.Dabria)
            {
                DabriaStarterRelic relic = FindObjectOfType<DabriaStarterRelic>();
                //effect after fight, not turn
                relic.DabRelicEffect();
            }

            turn = Turn.Enemy;
            EnemyTurn();
            //run banner "Enemy's Turn"

            //end of currentTurn - current turn = start status at next turn

            EffectDuration effectDuration = FindObjectOfType<EffectDuration>();
            effectDuration.RemoveTurn();

            for (int i = playerDeck.Count - 1; i >= 0; i--)
            {
                Card card = playerDeck[i];
                DiscardCard(card);
            }

            MoveCardToPullDeck();

            foreach (Transform cardOnHand in playerHand)
            {
                Destroy(cardOnHand.gameObject);
            }

            //if keep the block for next turn
            CheckBlockStatus();

            ShuffleDeck(5);
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
                //StartCoroutine(WaitForSeconds(2.5f));
            }

            //StartCoroutine(WaitForSeconds(2.5f));

            //run player's turn baner
            turn = Turn.Player;

            basePlayer = FindObjectOfType<BasePlayer>();
            effectDuration = FindObjectOfType<EffectDuration>();

            basePlayer.ResetEnergy();

            //check buffs & debuffs
            effectDuration.CheckLullaby();
            effectDuration.CheckToxic();
        }
    }

    public void MoveCardToPullDeck()
    {
        if (starterDeck.Count == 0)
        {
            for (int i = discardedDeck.Count - 1; i >= 0; i--)
            {
                Card card = discardedDeck[i];
                starterDeck.Add(card);
                discardedDeck.RemoveAt(i);
            }
        }
    }

    public void DiscardCard(Card card)
    {
        playerDeck.Remove(card);
        discardedDeck.Add(card);
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

        if (!effectManager.appliedStatus.Contains(effectManager.checkEffect("S10")))
        {
            //Debug.Log("Removing blocks");
            playerScript.RemoveBlock();
        }
        //Debug.Log("Do not remove block");

    }

    public void ResetDeck(Deckbuilding deck)
    {
        foreach (Card card in deck.tempList)
        {
            starterDeck.Add(card);
        }

        deck.tempList.Clear();
        deck.removed.Clear();

        if (playerDeck.Count == 0)
        {
            ShuffleDeck(5);
        }
    }

    public void openCard()
    {
        cardMenu.SetActive(true);
        Deckbuilding deckBuilding = FindObjectOfType<Deckbuilding>();

        if (starterDeck.Count != 0)
        {
            deckBuilding.SetUpItem();
        }
    }

    public void closeCard()
    {
        cardMenu.SetActive(false);
    }

    public int returnFloor()
    {
        return floorNumber;
    }

    public void FinishFight()
    {
        rewardPanel.SetActive(true);
        rewardSystem.GiveReward();
    }

    public void TurningOffReward()
    {
        if (rewardSystem.returnItemNumber() == 0)
        {
            rewardPanel.SetActive(false);

            //clear enemies
            foreach (Transform child in enemyHolder)
            {
                Destroy(child.gameObject);
            }

            //clear deck
            foreach (Transform child in playerHand)
            {
                Destroy(child.gameObject);
                OnDeckBehaviour cardBehave = child.gameObject.GetComponent<OnDeckBehaviour>();
                Card card = cardBehave.card;

                DiscardCard(card);
            }

            //new round
            currentFloor++;

            ShuffleDeck(5);
            SpawnEnemies();

            if (character == CharacterSelection.Character.Dabria)
            {
                Debug.Log("Healing up with Sparkling Hope");
                basePlayer.HealUp(2);
            }
        }
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        //Debug.Log("Paused");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
        //Debug.Log("Done Pause");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
