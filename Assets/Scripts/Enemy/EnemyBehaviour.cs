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
    public Slider healthSlider;

    public int block;
    public TextMeshProUGUI blockDisplay;

    public List<Sprite> M_Intention = new List<Sprite>();
    public List<Sprite> G_Intention = new List<Sprite>();
    public List<Sprite> E_Intention = new List<Sprite>();

    public GameObject intentionObject;
    public int nextActionValue = -1;
    public TextMeshProUGUI damageIntended;

    [SerializeField]private Animator anim;
    //public GameObject slashObject;
    //public Animator slashAnim;

    public GameObject panel;
    public TextMeshProUGUI nameOfEffect;
    public TextMeshProUGUI effectDes;

    //keep track of the the buff/debuff
    public List<Status> allStatus = new List<Status>();
    public List<Status> appliedStatus = new List<Status>();

    void Start()
    {
        anim = GetComponent<Animator>();
        currencyScript = FindObjectOfType<InGameCurrency>();
        player = FindObjectOfType<BasePlayer>();
        playerEff = FindObjectOfType<EffectDuration>();

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

        healthSlider.maxValue = enemyObject.maxHealth;
        healthSlider.minValue = 0;
    }

    void Update()
    {
        //shuffle actions (later improvement)

        healthDisplay.text = Mathf.Clamp(health, 0, enemyObject.maxHealth).ToString();
        blockDisplay.text = block.ToString();

        healthSlider.value = health;
    }

    public EnemyScriptableObject ChooseEnemies()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        int randValue = Random.Range(0, gm.enemyList.Count);

        return enemyObject = gm.enemyList[randValue];
    }

    public void TakeDamage(int damageAmount)
    {
        //slashAnim.SetTrigger("Slash");

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
            Debug.Log("is dead");
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
            
            //die and drop coin
            currencyScript.inGameCurrency += enemyObject.coinDrop;

            GameManager gm = FindObjectOfType<GameManager>();
            gm.enemyInStage.Remove(enemyObject);
            gm.CheckEnemies();
            gm.enemyInStage.Remove(enemyObject);
        }
    }

    public void ChooseNextAction()
    {
        Image thisImage = intentionObject.GetComponent<Image>();

        bool isSlept = appliedStatus.Contains(checkEffect("S06"));

        if (!isSlept)
        {
            //the enemy can hit the player for -- health or use special move
            if (nextActionValue == -1)
            {
                nextActionValue = GetRandomAction();
            }
            else
            {
                NextAction(enemyObject);
            }

            //choose next action value:
            nextActionValue = GetRandomAction();
            UpdateUIForNextAction();

            //change image to the next action
            switch (enemyObject.behaviourType)
            {
                case EnemyScriptableObject.EnemyType.Mushroom:
                    thisImage.sprite = M_Intention[nextActionValue];
                    break;

                case EnemyScriptableObject.EnemyType.Goblin:
                    thisImage.sprite = G_Intention[nextActionValue];
                    break;

                case EnemyScriptableObject.EnemyType.Eye:
                    thisImage.sprite = E_Intention[nextActionValue];
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

    public void NextAction(EnemyScriptableObject enemy)
    {
        switch (nextActionValue)
        {
            case 0:
                //Deal damage to player
                if (!appliedStatus.Contains(checkEffect("S02")))
                {
                    player.TakeDamage(enemyObject.damageDealt);
                }
                else
                {
                    player.TakeDamage(enemyObject.damageDealt + 2);

                    for(int i = 0; i < appliedStatus.Count; i++)
                    {
                        if (appliedStatus[i] == checkEffect("S02"))
                        {
                            appliedStatus.Remove(appliedStatus[i]); //remove strength upoon attacking
                        }
                    }
                }

                switch (enemyObject.behaviourType)
                {
                    case EnemyScriptableObject.EnemyType.Mushroom:
                        anim.SetTrigger("M_HandAttack");
                        anim.SetTrigger("M_Idle");
                        break;

                    case EnemyScriptableObject.EnemyType.Goblin:
                        anim.SetTrigger("G_BasicAttack");
                        anim.SetTrigger("G_Idle");
                        break;

                    case EnemyScriptableObject.EnemyType.Eye:
                        anim.SetTrigger("E_Attack1");
                        anim.SetTrigger("E_Idle");
                        break;
                }

                break;

            case 1:
                //Add defense
                block += 5;
                break;

            case 2:
                //apply toxic debuff on the player
                switch (enemy.behaviourType)
                {
                    case EnemyScriptableObject.EnemyType.Mushroom:
                        playerEff.UpdateEffectUI(checkEffect("S11"));
                        anim.SetTrigger("M_ToxicAttack");
                        anim.SetTrigger("M_Idle");
                        break;

                    case EnemyScriptableObject.EnemyType.Goblin:
                        playerEff.UpdateEffectUI(checkEffect("S12"));
                        anim.SetTrigger("G_BombAttack");
                        anim.SetTrigger("G_Idle");
                        break;

                    case EnemyScriptableObject.EnemyType.Eye:
                        playerEff.UpdateEffectUI(checkEffect("S13"));
                        anim.SetTrigger("E_Attack2");
                        anim.SetTrigger("E_Idle");
                        break;
                }
                break;

            case 3:
                //gain strength (+2 damage)
                appliedStatus.Add(checkEffect("S02"));
                break;
        }
    }

    public void UpdateUIForNextAction()
    {
        //check the next action to show data for attack/shield amount
        if (nextActionValue == 0)
        {
            nameOfEffect.text = "Attack!";

            if (!appliedStatus.Contains(checkEffect("S02")))
            {
                damageIntended.text = enemyObject.damageDealt.ToString();
                effectDes.text = "Attack the Player with " + enemyObject.damageDealt + " damage!";
            }
            else
            {
                damageIntended.text = (enemyObject.damageDealt + 2).ToString();
                effectDes.text = "Attack the Player with " + (enemyObject.damageDealt + 2) + " damage!";
            }
        }
        else if (nextActionValue == 1)
        {
            nameOfEffect.text = "Taking cover!";
            effectDes.text = "Grant the Enemy 5 Blocks.";

            damageIntended.text = "5";
        }
        else if (nextActionValue == 2)
        {
            //apply toxic debuff on the player
            switch (enemyObject.behaviourType)
            {
                case EnemyScriptableObject.EnemyType.Mushroom:
                    nameOfEffect.text = checkEffect("S11").effectName;
                    effectDes.text = checkEffect("S11").effectDescription;
                    break;

                case EnemyScriptableObject.EnemyType.Goblin:
                    nameOfEffect.text = checkEffect("S12").effectName;
                    effectDes.text = checkEffect("S12").effectDescription;
                    break;

                case EnemyScriptableObject.EnemyType.Eye:
                    nameOfEffect.text = checkEffect("S13").effectName;
                    effectDes.text = checkEffect("S13").effectDescription;
                    break;
            }

            damageIntended.text = "";
        }
        else if (nextActionValue == 3)
        {
            nameOfEffect.text = "Enrage";
            effectDes.text = "<size=70%> Grant the Enemy Strength. \n Strength: Increase Enemy damage by 2 until the next Enemy's attack";

            damageIntended.text = "";
        }
    }

    public int GetRandomAction()
    {
        if (!appliedStatus.Contains(checkEffect("S02")))
        {
            return Random.Range(0, 4);
        }
        else
            return Random.Range(0, 3); //except gaining strength
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

    public void HoverOnIntention()
    {
        panel.SetActive(true);
    }

    public void HoverOffIntention()
    {
        panel.SetActive(false);
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
    }
}
