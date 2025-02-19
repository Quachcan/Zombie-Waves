using Game.Scripts.EnemyScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EliteEnemy", menuName = "New Enemy/EliteEnemyConfig")]
public class EliteEnemyConfig : BaseEnemyConfig
{
    public string eliteName = "Elite";
    public float spawnRate = 1f;
}
