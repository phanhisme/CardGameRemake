using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public TextMeshProUGUI overHealthDisplay;
    public TextMeshProUGUI blockDisplay;
    public TextMeshProUGUI energyText;

    public Slider healthSlider;

    public int energy; //number of max turn depends on the character
    public int realmPower = 0; //power to use realm skills
    private int maxRealmPower;
    public int counter;

    public Animator anim;
    public GameObject losePanel;

    void Start()
    {
        effectScript = GetComponent<EffectDuration>();
        playerHealth = character.maxHealth;
        maxRealmPower = character.realmPower;

        healthSlider.maxValue = character.maxHealth;
        healthSlider.minValue = 0;

        losePanel.SetActive(false);
    }

    void Update()
    {
        healthDisplay.text = playerHealth.ToString();
        overHealthDisplay.text = playerHealth + "/" + character.maxHealth.ToString();
        healthSlider.value = playerHealth;

        blockDisplay.text = blockAmount.ToString();
        energyText.text = energy.ToString();

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm.turn == GameManager.Turn.Player)
        {
            anim.SetTrigger("isPlayerTurn");
        }
        else if (gm.turn == GameManager.Turn.Enemy)
        {
            anim.SetTrigger("isNotPlayerTurn");
        }
    }

    public void ResetToStart()
    {
        RemoveBlock();
        energy = character.energy;
        realmPower = 0;
        counter = 0;
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("isAttacked");

        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            if (!effectScript.ReBirth())
            {
                losePanel.SetActive(true);

                //Debug.Log("You lose...");

                //anim.SetTrigger("isDead");
                //Destroy(this.gameObject);

                //restart or return to main menu
            }
            else
            {
                playerHealth += 10;
                Debug.Log("You continue your journey... Mark of Rebirth is removed from your inventory");
                
                GameManager gm = FindObjectOfType<GameManager>();
                gm.markOfRebirth.SetActive(false);

                effectScript.appliedStatus.Remove(effectScript.checkEffect("S08"));
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