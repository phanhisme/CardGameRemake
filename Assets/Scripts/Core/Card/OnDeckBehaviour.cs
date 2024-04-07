using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeckBehaviour : MonoBehaviour
{
    private GameObject desPanel;

    private void Start()
    {
        desPanel = this.transform.GetChild(0).gameObject;
        desPanel.SetActive(false);
    }

    public void HoverOn()
    {
        //pop out description of the card here
        Debug.Log("hover on" + gameObject.name);

        desPanel.SetActive(true);
    }

    public void HoverOnNone()
    {
        Debug.Log("not hovering on anything");

        desPanel.SetActive(false);
    }
}
