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
            if (data.cardType == CardData.CardType.Attack && gameObject.tag == "Enemy")
            {
                EnemyBehaviour enemyScript = GetComponent<EnemyBehaviour>();

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

                            //it is only taking 1 enemy (DONT KNOW WHY???
                        }
                        break;

                    case 18:
                        Debug.Log(data.name + data.ID);
                        break;
                }
            }

            else if (data.cardType == CardData.CardType.Defense && gameObject.tag == "Player")
            {
                playerScript.AddBlock(data.effectAmount);
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