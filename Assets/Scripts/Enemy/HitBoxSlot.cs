using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitBoxSlot : MonoBehaviour, IDropHandler
{
    public BasePlayer playerScript;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<BasePlayer>();

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        //setting scriptable
        OnDeckBehaviour thisCard = gameManager.selectedCard;
        CardData data = thisCard.cardData;

        //Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && gameManager.energy >= data.stamCost)
        {
            //the card does not need to be in the perfect middle of the enemy, just hit the enemy will trigger the effect of the card upon dropping.
            //if the card is of attack type and is hitting the enmy
            if (gameObject.tag == "Enemy")
            {
                //find enemy behaviour within the dropped target
                EnemyBehaviour enemyScript = GetComponent<EnemyBehaviour>();
                
                //only attack cards
                if(data.cardType == CardData.CardType.Attack)
                {
                    //effect of the card on the enemy
                    switch (data.ID)
                    {
                        case 02: //Deal 4 damage to a single enemy
                            enemyScript.TakeDamage(data.effectAmount);
                            break;

                        case 07: //If there is 1 enemy, deal 24 damage. If more than 1, deal 12 each
                            if (gameManager.enemyInStage <= 1)
                            {
                                enemyScript.TakeDamage(data.effectAmount * 2); //24 damage
                            }

                            else if (gameManager.enemyInStage > 1)
                            {
                                GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
                                foreach (GameObject entity in enemy)
                                {
                                    EnemyBehaviour target = entity.GetComponent<EnemyBehaviour>();
                                    target.TakeDamage(data.effectAmount); //12 damage
                                }
                            }
                            break;

                        case 12: //Deal 6 Damage. If the target is inflicted by Endless Dream, deal double the damage and gain 1 Energy
                            bool enlessDream = enemyScript.appliedStatus.Contains(enemyScript.allStatus[1]);

                            //remember to remove the applied effects on the enemies!
                            Debug.Log(enlessDream);
                            if (enlessDream)
                            {
                                enemyScript.TakeDamage(data.effectAmount * 2);
                                gameManager.energy += 1;
                            }
                            else
                                enemyScript.TakeDamage(data.effectAmount);
                            break;

                        case 15: //Deal 6 Damage to all enemies. Heals Health equal to the unblocked damage dealt with this attack
                            GameObject[] aEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                            foreach (GameObject entity in aEnemy)
                            {
                                EnemyBehaviour target = entity.GetComponent<EnemyBehaviour>();

                                //record current health
                                int currentHealth = target.health;

                                //take damage
                                target.TakeDamage(data.effectAmount); //6 damage

                                //if the enemy lose health
                                if (target.health < currentHealth)
                                {
                                    //heal with the health lost from all the enemy
                                    playerScript.HealUp(currentHealth - target.health);
                                    Debug.Log(currentHealth - target.health);
                                }
                            }
                            break;

                        case 18: //Deal 4 damage to all enemies and gain 4 extra Skill cards. These cards deal 4 single damage and exhaust after use
                            GameObject[] bEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                            foreach (GameObject entity in bEnemy)
                            {
                                EnemyBehaviour target = entity.GetComponent<EnemyBehaviour>();
                                //take damage
                                target.TakeDamage(data.effectAmount); //4 damage
                                                                      //gain 4 cards (we do not have this card yet)
                                                                      //also change the card front to be other than "Main_Dream"
                            }
                            break;

                        default:
                            Debug.Log("There is no Attack cards with this ID: " + data.ID);
                            break;
                    }
                
                }
            }

            else if (gameObject.tag == "Player") //the reciever is player
            {
                if(data.cardType == CardData.CardType.Defense)
                {
                    switch (data.ID)
                    {
                        case 03: //Gain 5 Blocks
                            playerScript.AddBlock(data.effectAmount);
                            break;

                        case 04: //Gain 8 Blocks and draw a card
                            playerScript.AddBlock(data.effectAmount);
                            gameManager.ShuffleDeck(1); //draw a card
                            break;

                        case 10: //Can only be placed if there are at least 3 Block Card at hand.
                                 //All Blocksrecieved this turn will not be removed at the star of next turn

                            //Card defenseCard = data.cardType;
                            //foreach(CardData.CardType.Defense in gameManager.playerDeck)
                            //{

                            //}
                            break;

                        case 11: //Gain 6 and Moonlight effect for 2 turn
                            break;
                    }
                    
                }
                

            }

            //if the effect carry out -> turn decrease
            gameManager.selectedCard = null;

            //destroy card and effect pooof!
            //Instantiate(testPrefab, eventData.pointerDrag.transform.parent);
            Destroy(eventData.pointerDrag.gameObject);

            //cards needs to find their new locations since the first card is destroyed
            //thus, they spread differently
            OnDeckBehaviour[] remainingCards = FindObjectsOfType<OnDeckBehaviour>();

            foreach (OnDeckBehaviour card in remainingCards)
            {
                //resetting position of the remaining cards
                card.SetPosition();
            }

            //minus energy based on the stam cost
            gameManager.energy -= data.stamCost;
        }

        else if (gameManager.energy != 0 && gameManager.energy < data.stamCost)
        {
            Debug.Log("You do not have enough energy to use this card");
        }
        else if (gameManager.energy == 0)
        {
            Debug.Log("You ran out of energy, cannot place more cards");
        }
    }

    //public bool GainStrength()
    //{
    //    if()
    //}

    public void AddingStatus(Status status)
    {
        switch (status.statusID)
        {
            case "S02": //Strength: Gain 2 damage for physical attack / 2 turn
                break;

            case "S03": //Endless Dream: tag every enemies with Endless Dream, deal 6 dream damage if the player play attack card
                break;

            case "S04":
                break;

            case "S05":
                break;

            case "S06":
                break;

            default:
                Debug.Log("You are calling a null effect, check the code again");
                break;
        }
    }
}