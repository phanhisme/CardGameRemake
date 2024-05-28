using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDuration : MonoBehaviour
{
    public GameObject effectUIHolder;
    public GameObject effectPrefab;

    //keep track of the the buff/debuff
    public List<Status> allStatus = new List<Status>();
    public List<Status> appliedStatus = new List<Status>();

    public bool isNextTurn = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNextTurn)
        {
            isNextTurn = false;
            //decrease 1 duration of any active status

            bool eclipse = appliedStatus.Contains(allStatus[7]);
            //if contain eclipse, gain half of the block but double the attack in the next turn



            foreach (Status activeStatus in appliedStatus)
            {
                activeStatus.effectDuration--; //this might take the data number instead of the updated term
            }

        }

        
    }

    public void UpdateEffectUI(Status status)
    {
        if (appliedStatus.Contains(status))
        {
            StatusUI ui = GetComponentInChildren<StatusUI>();
            ui.chosenStatus=status;
            switch (ui.chosenStatus.statusID)
            {
                case "S02": //stregth
                    ui.currentDuration += 1;
                    ui.turnLeft.text = ui.currentDuration.ToString();
                    break;

                case "S09": //stregth
                    ui.currentDuration += 1;
                    ui.turnLeft.text = ui.currentDuration.ToString();
                    break;
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

            Instantiate(effectPrefab, effectUIHolder.transform);

            
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
}