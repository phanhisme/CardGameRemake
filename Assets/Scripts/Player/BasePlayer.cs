using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasePlayer : MonoBehaviour
{
    public ChooseCharacter character;

    [SerializeField] private int playerHealth;
    [SerializeField] private int blockAmount = 0;

    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;

    //keep track of the the buff/debuff
    public List<Status> allStatus = new List<Status>();
    public List<Status> appliedStatus = new List<Status>();

    void Start()
    {
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
            if (!ReBirth())
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
                appliedStatus.Remove(allStatus[0]);
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

    public int OverhealValue(int value)
    {
        return value - character.maxHealth;
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
