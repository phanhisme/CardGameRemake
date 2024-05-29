using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    public Status chosenStatus;

    public int initialDuration;
    public int currentDuration;

    public Image effectIcon;
    public TextMeshProUGUI turnLeft;

    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

    public void GetInitialData(Status status)
    {
        effectIcon.sprite = status.effectIcon;

        initialDuration = status.effectDuration;
        turnLeft.text = initialDuration.ToString();
        
        currentDuration = initialDuration;
        
        infoText.text = status.effectDescription;
    }

    public void GetInfoOnHover()
    {
        infoPanel.SetActive(true);
    }

    public void OffHover()
    {
        infoPanel.SetActive(false);
    }
}
