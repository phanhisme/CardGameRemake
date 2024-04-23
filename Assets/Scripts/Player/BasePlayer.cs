using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public ChooseCharacter character;

    [SerializeField] private int playerHealth;
    [SerializeField] //private int playerDamage; //this depends on the card the player use!

    void Start()
    {
        playerHealth = character.maxHealth;
    }

    void Update()
    {
        
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
}