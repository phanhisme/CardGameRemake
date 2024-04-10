using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;

    public int maxHealth;
    public int damageDealt;

    public int coinDrop;

}
