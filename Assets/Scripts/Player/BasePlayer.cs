using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasePlayer : MonoBehaviour
{
    public ChooseCharacter character;
    [SerializeField]private EffectDuration effectScript;

    [SerializeField] private int playerHealth;
    [SerializeField] private int blockAmount = 0;

    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;

    void Start()
    {
        effectScript = GetComponent<EffectDuration>();
        playerHealth = character.maxHealth;
    }

    void Update()
    {
        text.text = playerHealth.ToString();
        text2.text = blockAmount.ToString();
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            if (!effectScript.ReBirth())
            {
                Debug.Log("You lose...");

                //anim.SetTrigger("isDead");
                //Destroy(this.gameObject);

                //restart or return to main menu
            }
            else
            {
                playerHealth += 10;
                Debug.Log("You continue your journey");
                effectScript.appliedStatus.Remove(effectScript.allStatus[0]);
            }

        }
    }

    public void AddBlock(int amount)
    {
        blockAmount += amount;
    }

    public void RemoveBlock()
    {
        //at the start of turn, remove block
        blockAmount = 0;
    }

    public void HealUp(int health)
    {
        playerHealth += health;
    }

    public bool HealthCheck()
    {
        if (playerHealth >= 50)
        {
            return true;
        }
        else
            return false;
    }

    public int OverhealValue()
    {
        if (playerHealth <= character.maxHealth)
        {
            //do not trigger lullaby
            return 0;
        }
        else
            return playerHealth - character.maxHealth; //effect value = overheal amount
    }

    public void TakeHealth(int health)
    {
        if (playerHealth >= 1)
        {
            playerHealth -= health;
        }
        else
            return;
    }
}
