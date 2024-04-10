using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnDeckBehaviour : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private GameManager gameManager;
    private HitBoxSlot enemyHitBox;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField] private float moveDistance = 100f;

    //public bool checkForCardLocation = false;
    //public Card cardObject;
    private Vector2 originalPosition;
    
    //public GameObject cardClone;
    //public Transform cloneSpawner;
    

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameManager = FindObjectOfType<GameManager>();
        enemyHitBox = FindObjectOfType<HitBoxSlot>();

        StartCoroutine(GetCardPosition());
    }

    private void Update()
    {
        if (enemyHitBox.cardNeedsRelocate)
        {
            //relocate the cards (after consuming the first card)
            StartCoroutine(GetCardPosition());
            enemyHitBox.cardNeedsRelocate = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //On mouse down
        Debug.Log("OnPointerDown");
        gameManager.selectedCard = this;

        //instantiate a clone that takes the original place of this object
        //this clone spawner needs to take the original place of this card
        //GameObject cloneImage = Instantiate(cardClone, originalPosition.anchoredPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //When the player start dragging the card
        Debug.Log("OnBeginDrag");

        //Transparent effect
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //When the player let go of the card
        Debug.Log("OnEndDrag");

        // return to the original transform
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.localPosition = originalPosition;
        //StartCoroutine(ReturnCard());
    }

    public void OnDrag(PointerEventData eventData)
    {
        //On Drag updates each frame and monitor where the mouse is 
        //delta might be the problem to fix later. Divided by scale factor so that the scale of the canvas will not mess up the transform of the dragged object
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void HoverOn()
    {
        //pop out description of the card here
        Debug.Log("hover on" + gameObject.name);

        //card will move upwards and widen to help the player look at the effect better
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + moveDistance);
    }

    public void HoverOff()
    {
        //Debug.Log("hover off " + gameObject.name);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - moveDistance);

    }

    public IEnumerator GetCardPosition()
    {
        yield return new WaitForEndOfFrame();
        originalPosition = rectTransform.localPosition;
    }
}