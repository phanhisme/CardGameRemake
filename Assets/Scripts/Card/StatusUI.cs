using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    public Status chosenStatus;

    [SerializeField] private int currentDuration;

    public Image effectIcon;
    public TextMeshProUGUI turnLeft;

    public void GetInitialData()
    {
        currentDuration = chosenStatus.effectDuration;
        effectIcon.sprite = chosenStatus.effectIcon;
    }

    public void Update()
    {
        turnLeft.text = currentDuration.ToString();
        //GameManager gm = FindObjectOfType<GameManager>();

        if (chosenStatus != null)
        {
            
        }

        if (currentDuration == 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddTurn()
    {
        currentDuration += 1;
    }
}
