using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitBoxSlot : MonoBehaviour, IDropHandler
{
    private EnemyBehaviour enemyScript;
    public BasePlayer playerScript;
    private GameManager gameManager;


    private void Start()
    {
        //playerScript = FindObjectOfType<BasePlayer>();
        enemyScript = FindObjectOfType<EnemyBehaviour>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && gameManager.energy > 0)
        {
            //the card does not need to be in the perfect middle of the enemy, just hit the enemy will trigger the effect of the card upon dropping.

            //setting scriptable
            OnDeckBehaviour thisCard = gameManager.selectedCard;
            CardData data = thisCard.cardData;

            //if the card is of attack type and is hitting the enmy
            if (data.cardType == CardData.CardType.Attack && gameObject.tag == "Enemy")
            {
                //effect of the card on the enemy
                enemyScript.TakeDamage(data.effectAmount);
            }

            else if(data.cardType == CardData.CardType.Defense && gameObject.tag == "Player")
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
            gameManager.energy -= 1;
        }

        else if (gameManager.energy <= 0)
        {
            Debug.Log("You ran out of energy, cannot place more cards");
        }
    }
}