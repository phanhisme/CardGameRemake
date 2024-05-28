using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    public Status chosenStatus;
    public int currentDuration;

    public Image effectIcon;
    public TextMeshProUGUI turnLeft;

    public void GetInitialData(Status status)
    {
        currentDuration = status.effectDuration;
        turnLeft.text = currentDuration.ToString();
        effectIcon.sprite = status.effectIcon;
    }

    public void Update()
    {

    }

    public void AddTurn(Status status)
    {
        currentDuration += 1;
        turnLeft.text = currentDuration.ToString();
        Debug.Log("Turn of " + status + "is now " + currentDuration);
    }
}
