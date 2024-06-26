using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitBoxSlot : MonoBehaviour, IDropHandler
{
    public BasePlayer playerScript;
    public EffectDuration effectScript;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<BasePlayer>();
        effectScript = player.GetComponent<EffectDuration>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        //setting scriptable
        OnDeckBehaviour thisCard = gameManager.selectedCard;
        CardData data = thisCard.cardData;
        Card card = thisCard.card;

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
                if (data.cardTarget == CardData.CardTarget.Enemy)
                {
                    bool strengthActive = effectScript.appliedStatus.Contains(effectScript.allStatus[0]);
                    if (strengthActive)
                    {
                        data.effectAmount += 2;
                    }

                    //effect of the card on the enemy
                    switch (data.ID)
                    {
                        case 02: //Deal 4 damage to a single enemy
                            enemyScript.TakeDamage(data.effectAmount);
                            break;

                        case 07: //If there is 1 enemy, deal 24 damage. If more than 1, deal 12 each
                            if (gameManager.enemyInStage.Count <= 1)
                            {
                                enemyScript.TakeDamage(data.effectAmount * 2); //24 damage
                            }

                            else if (gameManager.enemyInStage.Count > 1)
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

                        case 19: //Sleep a single enemy for 2 turn. Only wake up if attacked by the player or end of Sleep
                            enemyScript.appliedStatus.Add(enemyScript.allStatus[2]);
                            break;

                        default:
                            Debug.Log("There is no Attack cards with this ID: " + data.ID);
                            break;
                    }

                }

                else
                    return;
            }

            else if (gameObject.tag == "Player") //the reciever is player
            {
                if(data.cardTarget == CardData.CardTarget.Player)
                {
                    //set status
                    Status strength = effectScript.allStatus[0];
                    Status moonlight = effectScript.allStatus[2];
                    Status lullaby = effectScript.allStatus[3];
                    Status eclipse = effectScript.allStatus[6];
                    Status remainBlock = effectScript.allStatus[7];

                    bool eclipseActive = effectScript.appliedStatus.Contains(eclipse);
                    if (eclipseActive)
                    {
                        if (data.cardType == CardData.CardType.Defense)
                        {
                            data.effectAmount = data.effectAmount / 2;
                        }
                        else if (data.cardType == CardData.CardType.Attack)
                        {
                            data.effectAmount = data.effectAmount * 2;
                        }
                    }

                    bool lullabyActive = effectScript.appliedStatus.Contains(lullaby);
                    if (lullabyActive)
                    {
                        int lullabyValue = playerScript.OverhealValue();
                        //only give effect if player is 
                        if (lullabyValue > 0)
                        {
                            //get lullaby effect
                            if (data.cardType == CardData.CardType.Defense)
                            {
                                data.effectAmount += lullabyValue;
                                lullabyValue = 0;
                                Debug.Log(lullabyValue);
                            }
                        } 
                    }

                    switch (data.ID)
                    {
                        //DEFENSE

                        case 03: //Gain 5 Blocks
                            playerScript.AddBlock(data.effectAmount);
                            break;

                        case 04: //Gain 8 Blocks and draw a card
                            playerScript.AddBlock(data.effectAmount);
                            gameManager.ShuffleDeck(1); //draw a card
                            break;

                        case 10: //Can only be placed if there are at least 3 Block Card at hand.
                                 //All Blocks recieved this turn will not be removed at the star of next turn

                            int defenseCards = gameManager.GetDefenseCard();
                            

                            if (effectScript.ReturnCard(remainBlock))
                            {
                                Debug.Log("This effect will not stack, return card");
                                return;
                            }
                            else
                            {
                                if (defenseCards >= 3)
                                {
                                    //add status
                                    effectScript.UpdateEffectUI(remainBlock);
                                }
                                else
                                    return;
                            }
                            break;

                        case 11: //Gain 6 and Moonlight effect for 2 turn
                            playerScript.AddBlock(data.effectAmount);

                            //this is very hard coded YES
                            
                            effectScript.appliedStatus.Add(moonlight);
                            break;

                        //SKILLS

                        case 05: //Gain half of the Blocks but double the Attack
                           
                            if (effectScript.ReturnCard(eclipse))
                            {
                                Debug.Log("This effect is already active and cannot stack, returning card");
                                return;
                            }
                            else
                                effectScript.UpdateEffectUI(eclipse);
                            break;

                        case 06: //Gain Strength for 2 turn
                            
                            effectScript.UpdateEffectUI(strength);
                            break;

                        case 08: //take 1 health and draw a card
                            playerScript.TakeDamage(1);
                            gameManager.ShuffleDeck(1); //"DRAW"
                            break;

                        case 13://Heals 2 Health at the end of turn for 3 turns.
                                //If the player is at max health, the amount overheal will grant the player Lullaby effect
                            if (effectScript.ReturnCard(lullaby))
                            {
                                Debug.Log("This effect is already active and cannot stack, returning card");
                                return;
                            }
                            else
                            {
                                playerScript.HealUp(2);
                                effectScript.UpdateEffectUI(lullaby);
                            }

                            break;

                        case 20://From a cocoon to Butterfly, if the player is defeated, they will restore 10 Health and return to the battle.
                                //Recieve "Mark of Rebirth" (until reborn)
                            effectScript.UpdateEffectUI(effectScript.allStatus[6]);
                            break;

                        default:
                            return;
                    }
                }

                else
                {
                    return;
                }
            }

            //if the effect carry out -> turn decrease
            gameManager.selectedCard = null;

            //destroy card and effect pooof!
            //Instantiate(testPrefab, eventData.pointerDrag.transform.parent);
            Destroy(eventData.pointerDrag.gameObject);

            //discard cards
            gameManager.discardedDeck.Add(card);
            gameManager.playerDeck.Remove(card);

            //cards needs to find their new locations since the first card is destroyed
            //thus, they spread differently
            OnDeckBehaviour[] remainingCards = FindObjectsOfType<OnDeckBehaviour>();

            foreach (OnDeckBehaviour onDeck in remainingCards)
            {
                //resetting position of the remaining cards
                onDeck.SetPosition();
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
}