using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyScriptableObject enemyObject;
    private InGameCurrency currencyScript;

    private Image enemyImage;
    public TextMeshProUGUI enemyName;

    public int health; //keep track of the enemy, using to only keep the max health of the enemy and not the current health
    public TextMeshProUGUI healthDisplay;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        currencyScript = FindObjectOfType<InGameCurrency>();

        health = enemyObject.maxHealth;

        enemyName.text = enemyObject.enemyName;
    }

    void Update()
    {
        //shuffle actions (later improvement)

        healthDisplay.text = health.ToString();
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

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
}
