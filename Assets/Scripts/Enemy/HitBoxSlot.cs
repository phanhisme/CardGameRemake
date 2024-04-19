using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitBoxSlot : MonoBehaviour, IDropHandler
{
    private EnemyBehaviour enemyScript;
    private GameManager gameManager;
    public bool cardNeedsRelocate = false;
    public GameObject testPrefab;

    private void Start()
    {
        enemyScript = GetComponent<EnemyBehaviour>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && gameManager.energy > 0)
        {
            //the card does not need to be in the perfect middle of the enemy, just hit the enemy will trigger the effect of the card upon dropping.

            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            Debug.Log(eventData.pointerDrag.name + " is being dropped on " + this.gameObject.name);

            //effect of the card
            enemyScript.TakeDamage(10);

            //if the effect carry out -> turn decrease
            gameManager.selectedCard = null;

            //destroy card and effect pooof!
            //Instantiate(testPrefab, eventData.pointerDrag.transform.parent);
            Destroy(eventData.pointerDrag.gameObject);

            //cards needs to find their new locations since the first card is destroyed
            //thus, they spread differently

            cardNeedsRelocate = true;
            gameManager.energy -= 1;
        }
        else if (gameManager.energy <= 0)
        {
            Debug.Log("You ran out of energy, cannot place more cards");
        }
    }
}
