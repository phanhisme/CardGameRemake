using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyScriptableObject enemyObject;
    private InGameCurrency currencyScript;
    public BasePlayer player;

    private Image enemyImage;
    public TextMeshProUGUI enemyName;

    public int health; //keep track of the enemy, using to only keep the max health of the enemy and not the current health
    public TextMeshProUGUI healthDisplay;

    public int block;
    public TextMeshProUGUI blockDisplay;

    private Animator anim;

    //keep track of the the buff/debuff
    public List<Status> allStatus = new List<Status>();
    public List<Status> appliedStatus = new List<Status>();

    void Start()
    {
        anim = GetComponent<Animator>();
        currencyScript = FindObjectOfType<InGameCurrency>();
        //player = FindObjectOfType<BasePlayer>();

        health = enemyObject.maxHealth;
        
        enemyName.text = enemyObject.enemyName;

    }

    void Update()
    {
        //shuffle actions (later improvement)

        healthDisplay.text = health.ToString();
        blockDisplay.text = block.ToString();
    }

    public void TakeDamage(int damageAmount)
    {
        //if block available
        if (block >= 0)
        {
            int negativeBlock = block - damageAmount;
            if (negativeBlock < 0)
            {
                //block is negative, minus the remaining to health
                health += negativeBlock;

                //set block back to 0
                block = 0;
            }

        }
        else
        {
            health -= damageAmount;
        }

        if(health <= 0)
        {
            //die and drop coin
            currencyScript.inGameCurrency += enemyObject.coinDrop;

            Debug.Log("is dead");
            //anim.SetTrigger("isDead");

            //Destroy(this.gameObject);

            //move to next stage (choose the new path through story?)
        }
    }

    public void ChooseNextAction()
    {
        bool isSlept = appliedStatus.Contains(allStatus[2]);

        if (!isSlept)
        {
            //the enemy can hit the player for -- health or use special move
            int rand = GetRandomAction();

            switch (rand)
            {
                case 0:
                    //Deal damage to player
                    player.TakeDamage(enemyObject.damageDealt);
                    break;

                case 1:
                    //Add defense
                    block += 5;
                    Debug.Log("new block = " + block);
                    break;

                case 2:
                    //Gain Strength
                    break;

                case 3:
                //...
                default:
                    Debug.Log("enemy - failed to choose action");
                    break;
            }
        }
        else
        {
            Debug.Log("This enemy is slept, skipping Turn");
            return;
        }
       

        //while the enemy is above 50 health: 20% to buff, 35% to attack, 25% to def,20% to total random
        //after attacking, the chance to use defense is increased to 40%, random to attack

        //under 50 health, chance to heal up: 20%

        
        
    }

    public int GetRandomAction()
    {
        return Random.Range(0, 1);
    }
}
