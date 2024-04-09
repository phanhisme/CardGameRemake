using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitBoxSlot : MonoBehaviour, IDropHandler
{
   public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            //the card does not need to be in the perfect middle of the enemy, just hit the enemy will trigger the effect of the card upon dropping.

            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            Debug.Log(eventData.pointerDrag.name + " is being dropped on " + this.gameObject.name);

            //effect of the card

            //destroy card and effect pooof!
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}
