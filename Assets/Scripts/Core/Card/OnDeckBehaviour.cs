using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDeckBehaviour : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private GameObject desPanel;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField] private float moveDistance = 100f;

    private void Awake()
    {
        desPanel = this.transform.GetChild(0).gameObject;
        desPanel.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //On mouse down
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //When the player start dragging the card
        Debug.Log("OnBeginDrag");

        //instantiate a clone that takes the original place of this object
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        //On Drag updates each frame and monitor where the mouse is 
        Debug.Log("OnDrag");

        //delta might be the problem to fix later. Divided by scale factor so that the scale of the canvas will not mess up the transform of the dragged object
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void HoverOn()
    {
        //pop out description of the card here
        Debug.Log("hover on" + gameObject.name);

        //card will move upwards and widen to help the player look at the effect better
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + moveDistance);

        Debug.Log(rectTransform.anchoredPosition);
        //desPanel.SetActive(true);
    }

    public void HoverOff()
    {
        Debug.Log("hover off " + gameObject.name);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - moveDistance);

        //desPanel.SetActive(false);
    }

    public void HoverOnNone()
    {
        Debug.Log("not hovering on anything");

    }
}
