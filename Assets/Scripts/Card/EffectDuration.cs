using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDuration : MonoBehaviour
{
    public GameObject effectUIHolder;
    public GameObject effectPrefab;
    public BasePlayer playerScript;

    //keep track of the the buff/debuff
    public List<Status> allStatus = new List<Status>();
    public List<Status> appliedStatus = new List<Status>();

    public void UpdateEffectUI(Status status)
    {
        if (appliedStatus.Contains(status))
        {
            foreach(Transform child in effectUIHolder.transform)
            {
                StatusUI ui = child.GetComponent<StatusUI>();
                if (ui.chosenStatus.statusID == status.statusID)
                {
                    switch (status.statusID)
                    {
                        case "S02": //strength can add
                            ui.currentDuration += 1;
                            ui.turnLeft.text = ui.currentDuration.ToString();
                            break;

                        default:
                            Debug.Log("Error, cannot find this status");
                            break;
                    }

                    
                }
            }

            //ui.currentDuration += 1;
            //ui.turnLeft.text = ui.currentDuration.ToString();
        }
        else
        {
            Debug.Log("Add a new status to target");

            //create the icon with its duration number
            appliedStatus.Add(status);

            StatusUI ui = effectPrefab.GetComponent<StatusUI>();
            ui.GetInitialData(status);
            ui.chosenStatus = status;

            Instantiate(effectPrefab, effectUIHolder.transform);
        }
    }

    public void RemoveTurn()
    {
        foreach(Transform child in effectUIHolder.transform)
        {
            StatusUI ui = child.GetComponent<StatusUI>();
            ui.currentDuration -= 1;
            ui.turnLeft.text = ui.currentDuration.ToString();

            if (ui.currentDuration == 0)
            {
                if (ui.chosenStatus.statusID == "S05")
                {
                    
                    playerScript.HealUp(playerScript.OverhealValue());
                    Debug.Log("Heal at the end of turn with" + playerScript.OverhealValue());
                }

                appliedStatus.Remove(ui.chosenStatus);
                Destroy(child.gameObject);
            }
        }
    }

    public void CheckLullaby()
    {
        if (appliedStatus.Contains(allStatus[3]))
        {
            playerScript.HealUp(10);
            Debug.Log("Lullaby Value is " + playerScript.OverhealValue());
        }
    }

    public bool ReBirth()
    {
        if (appliedStatus.Contains(allStatus[6]))
        {
            return true;
        }
        else
            return false;
    }

    public bool ReturnCard(Status status)
    {
        if (appliedStatus.Contains(status))
        {
            return true;
        }
        else
            return false;
    }
}