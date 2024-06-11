using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasePlayer : MonoBehaviour
{
    public List<GameObject> realmOrb = new List<GameObject>();
    public GameObject orbs;
    public Transform _orbHolder;

    public ChooseCharacter character;
    [SerializeField]private EffectDuration effectScript;

    [SerializeField] private int playerHealth;
    [SerializeField] private int blockAmount = 0;

    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI blockDisplay;
    public TextMeshProUGUI energyText;

    public int energy; //number of max turn depends on the character
    public int realmPower; //power to use realm skills
    private int maxRealmPower;
    public int counter;

    void Start()
    {
        effectScript = GetComponent<EffectDuration>();
        playerHealth = character.maxHealth;
        maxRealmPower = character.realmPower;
    }

    void Update()
    {
        healthDisplay.text = playerHealth.ToString();
        blockDisplay.text = blockAmount.ToString();
        energyText.text = energy.ToString();
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
                Debug.Log("You continue your journey... Mark of Rebirth is removed from your inventory");
                //effectScript.appliedStatus.Remove(effectScript.allStatus[]);
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

    public void RealmPower()
    {
        if (counter < 1)
        {
            realmPower++;
            counter++;
        }
        else if (counter == 1)
        {
            counter = 0;
            if (realmPower <= maxRealmPower)
            {
                realmPower++;

                GameObject orb = Instantiate(orbs, _orbHolder);
                realmOrb.Add(orb);
            }
        }
    }

    public void ResetEnergy()
    {
        energy = character.energy;
    }
}