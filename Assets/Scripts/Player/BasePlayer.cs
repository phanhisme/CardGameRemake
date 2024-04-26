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

    [SerializeField] //private int playerDamage; //this depends on the card the player use!

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
            Debug.Log("You lose...");
            
            //anim.SetTrigger("isDead");
            //Destroy(this.gameObject);

            //restart or return to main menu
        }
    }

    public void AddBlock(int amount)
    {
        blockAmount += amount;
    }

    public void RelicHealUp(int health)
    {
        playerHealth += health;
    }
}
