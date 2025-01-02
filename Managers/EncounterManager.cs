using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AnfBattleSystem;
using NUnit.Framework;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] public List<BaseEnemy> ActiveEnemies = new List<BaseEnemy>();
    [SerializeField] public List<EnemySO> enemySOs; // Reference to your HeroSOs


    public void StartEncounter()
    {
        int numEnemies = Random.Range(1, 5); // Randomly choose between 1 and 4 enemies
        Debug.Log("Starting encounter with " + numEnemies + " enemies.");

        for (int i = 0; i < numEnemies; i++)
        {
            EnemySO enemySO = enemySOs[i];
            GameObject enemyObject = new GameObject($"Enemy_{i}");
            BaseEnemy enemy = enemyObject.AddComponent<BaseEnemy>(); // Add the BaseEnemy component
            enemy.InitializeEnemy(enemySO);
            ActiveEnemies.Add(enemy);
            enemy.UpdateStats();    

            Debug.Log("Enemy initialized: " + enemy.enemySO.enemyName + " with HP=" + enemy.maxHP);
        }
    }

   }

 
