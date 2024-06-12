using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyScriptableObject enemyObject;
    private InGameCurrency currencyScript;

    private BasePlayer player;
    private EffectDuration playerEff;

    public Image enemyImage;
    public TextMeshProUGUI enemyName;

    public int health; //keep track of the enemy, using to only keep the max health of the enemy and not the current health
    public TextMeshProUGUI healthDisplay;

    public int block;
    public TextMeshProUGUI blockDisplay;

    public List<Sprite> intention = new List<Sprite>();
    public GameObject intentionObject;
    public int nextActionValue = -1;

    [SerializeField]private Animator anim;
    public GameObject slashObject;
    public Animator slashAnim;

    //keep track of the the buff/debuff
    public List<Status> allStatus = new List<Status>();
    public List<Status> appliedStatus = new List<Status>();

    void Start()
    {
        anim = GetComponent<Animator>();
        currencyScript = FindObjectOfType<InGameCurrency>();
        player = FindObjectOfType<BasePlayer>();

        enemyObject = ChooseEnemies();

        switch (enemyObject.behaviourType)
        {
            case EnemyScriptableObject.EnemyType.Mushroom:
                anim.SetTrigger("M_Idle");
                break;

            case EnemyScriptableObject.EnemyType.Eye:
                anim.SetTrigger("E_Idle");
                break;

            case EnemyScriptableObject.EnemyType.Goblin:
                anim.SetTrigger("G_Idle");
                break;
        }

        health = enemyObject.maxHealth;
        enemyName.text = enemyObject.enemyName;
        enemyImage.sprite = enemyObject.enemyImage;

        ChooseNextAction();
    }

    void Update()
    {
        //shuffle actions (later improvement)

        healthDisplay.text = health.ToString();
        blockDisplay.text = block.ToString();
    }

    public EnemyScriptableObject ChooseEnemies()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        int randValue = Random.Range(0, gm.enemyList.Count);

        return enemyObject = gm.enemyList[randValue];
    }

    public void TakeDamage(int damageAmount)
    {
        slashObject.SetActive(true);
        slashAnim.SetTrigger("Slash");

        switch (enemyObject.behaviourType)
        {
            case EnemyScriptableObject.EnemyType.Mushroom:
                anim.SetTrigger("M_TakingDamage");
                anim.SetTrigger("M_Idle");
                break;

            case EnemyScriptableObject.EnemyType.Eye:
                anim.SetTrigger("E_TakingDamage");
                anim.SetTrigger("E_Idle");
                break;

            case EnemyScriptableObject.EnemyType.Goblin:
                anim.SetTrigger("G_TakingDamage");
                anim.SetTrigger("G_Idle");
                break;
        }

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
            StartCoroutine(WaitForSeconds(0.1f));
            switch (enemyObject.behaviourType)
            {
                case EnemyScriptableObject.EnemyType.Mushroom:
                    anim.SetTrigger("M_Death");
                    break;

                case EnemyScriptableObject.EnemyType.Eye:
                    anim.SetTrigger("E_Death");
                    break;

                case EnemyScriptableObject.EnemyType.Goblin:
                    anim.SetTrigger("G_Death");
                    break;
            }
            StartCoroutine(WaitForSeconds(0.5f));
            

            //die and drop coin
            currencyScript.inGameCurrency += enemyObject.coinDrop;

            GameManager gm = FindObjectOfType<GameManager>();
            gm.enemyInStage.Remove(enemyObject);
            gm.CheckEnemies();
            Destroy(this.gameObject);
        }
    }

    public void DisableSlash()
    {
        Debug.Log("disable");
        slashAnim.SetTrigger("Empty");
        slashObject.SetActive(false);
    }

    public void ChooseNextAction()
    {
        bool isSlept = appliedStatus.Contains(checkEffect("S06"));

        if (!isSlept)
        {
            switch (enemyObject.behaviourType)
            {
                case EnemyScriptableObject.EnemyType.Mushroom:

                    //the enemy can hit the player for -- health or use special move
                    if (nextActionValue == -1)
                    {
                        nextActionValue = GetRandomAction();
                    }

                    switch (nextActionValue)
                    {
                        case 0:
                            //Deal damage to player
                            player.TakeDamage(enemyObject.damageDealt);
                            break;

                        case 1:
                            //Add defense
                            block += 5;
                            break;

                        case 2:
                            //Gain Strength
                            appliedStatus.Add(checkEffect("S02"));
                            break;

                        case 3:
                            //apply toxic debuff on the player
                            playerEff.appliedStatus.Add(checkEffect("S11"));
                            break;

                        default:
                            Debug.Log(enemyName + "- failed to choose action");
                            break;
                    }

                    //choose next action value:
                    nextActionValue = GetRandomAction();
                    Image thisImage = intentionObject.GetComponent<Image>();
                    thisImage.sprite = intention[nextActionValue];
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

    public Status checkEffect(string ID)
    {
        foreach(Status status in allStatus)
        {
            if (status.statusID == ID)
            {
                return status;
            }
        }

        return null;
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
    }
}
