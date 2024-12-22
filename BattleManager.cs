using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnfBattleSystem;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public PartyManager partyManager;
    public EncounterManager encounterManager;
    public UIManager uiManager;
    public BattleStateMachine bsm;
    public static BattleManager instance;
    public GameObject selectorPrefab;
    public GameObject heroPrefab;
    public GameObject enemyPrefab;

    private static readonly Vector3[] heroPositions = new Vector3[]
    {
        new Vector3(-5.5f, 1.02f, 3.37f),
        new Vector3(3.9f, 1.02f, 3.37f),
        new Vector3(9.1f, 1.02f, 3.37f),
        new Vector3(14f, 1.02f, 3.37f)
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (partyManager != null)
        {
            StartBattle();
            Debug.Log("FIGHT!!!");
        }
    }

     public void StartBattle()
    {
        InitializeParty();
        InitializeEnemies();
        InitializeUI();
    }

    private void InitializeParty()
    {
        partyManager.InitializeParty();

        if (partyManager.partyMembers == null || partyManager.partyMembers.Count == 0)
        {
            Debug.LogError("No party members found in PartyManager.");
            return;
        }

        TurnManager turnManager = TurnManager.instance;

        for (int i = 0; i < partyManager.partyMembers.Count; i++)
        {
            BaseHero hero = partyManager.partyMembers[i];
            Vector3 position = i < heroPositions.Length ? heroPositions[i] : Vector3.zero;
            GameObject heroGameObject = CreateHeroPrefab(hero, position);

            // Add the hero prefab to TurnManager's heroesInBattle list
            if (turnManager != null)
            {
                turnManager.heroesInBattle.Add(heroGameObject);
            }
            else
            {
                Debug.LogError("TurnManager instance not found.");
            }
        }
    }

    private GameObject CreateHeroPrefab(BaseHero hero, Vector3 position)
    {
        GameObject heroGameObject = Instantiate(heroPrefab);
        heroGameObject.name = hero.heroSO.heroName;
        heroGameObject.transform.position = position;

        BaseHero newHero = heroGameObject.AddComponent<BaseHero>();
        newHero.InitializeHero(hero.heroSO);

        HeroStateMachine hsm = heroGameObject.AddComponent<HeroStateMachine>();
        hsm.Initialize(newHero, uiManager);

        GameObject selector = FindChildByName(heroGameObject.transform, "Selector");
        if (selector != null)
        {
            hsm.Selector = selector;
            selector.SetActive(false);
        }
        else
        {
            Debug.LogError("Selector not found in hero prefab's children.");
        }

        hsm.enabled = true;

        return heroGameObject;
    }

    private void InitializeEnemies()
    {
        encounterManager.StartEncounter();

        if (encounterManager.ActiveEnemies == null || encounterManager.ActiveEnemies.Count == 0)
        {
            Debug.LogError("No active enemies found in EncounterManager.");
            return;
        }

        TurnManager turnManager = TurnManager.instance;
        Vector3[] enemyPositions = GetEnemyPositions(encounterManager.ActiveEnemies.Count);

        for (int i = 0; i < encounterManager.ActiveEnemies.Count; i++)
        {
            BaseEnemy enemy = encounterManager.ActiveEnemies[i];
            Vector3 position = i < enemyPositions.Length ? enemyPositions[i] : Vector3.zero;
            GameObject enemyGameObject = CreateEnemyPrefab(enemy, position);

            // Add the enemy prefab to TurnManager's enemiesInBattle list
            if (turnManager != null)
            {
                turnManager.enemiesInBattle.Add(enemyGameObject);
            }
            else
            {
                Debug.LogError("TurnManager instance not found.");
            }
        }
    }

    private GameObject CreateEnemyPrefab(BaseEnemy enemy, Vector3 position)
    {
        GameObject enemyGameObject = Instantiate(enemyPrefab);
        enemyGameObject.name = enemy.enemySO.enemyName;
        enemyGameObject.transform.position = position;

        BaseEnemy newEnemy = enemyGameObject.AddComponent<BaseEnemy>();
        newEnemy.InitializeEnemy(enemy.enemySO);

        EnemyStateMachine esm = enemyGameObject.AddComponent<EnemyStateMachine>();
        esm.Initialize(newEnemy);

        GameObject selector = FindChildByName(enemyGameObject.transform, "Selector");
        if (selector != null)
        {
            esm.Selector = selector;
            selector.SetActive(false);
        }
        else
        {
            Debug.LogError("Selector not found in enemy prefab's children.");
        }

        esm.enabled = true;

        return enemyGameObject;
    }

    private Vector3[] GetEnemyPositions(int enemyCount)
    {
        switch (enemyCount)
        {
            case 1:
                return new Vector3[]
                {
                    new Vector3(6.36f, 1.02f, 14f)
                };
            case 2:
                return new Vector3[]
                {
                    new Vector3(9.1f, 1.02f, 14f),
                    new Vector3(3.9f, 1.02f, 14f)
                };
            case 3:
                return new Vector3[]
                {
                    new Vector3(6.36f, 1.02f, 14f),
                    new Vector3(9.1f, 1.02f, 14f),
                    new Vector3(14f, 1.02f, 14f)
                };
            case 4:
                return new Vector3[]
                {
                    new Vector3(3.9f, 1.02f, 14f),
                    new Vector3(9.1f, 1.02f, 14f),
                    new Vector3(-5.5f, 1.02f, 14f),
                    new Vector3(14f, 1.02f, 14f)
                };
            default:
                return new Vector3[]
                {
                    new Vector3(4.2f, 1.02f, 14f),
                    new Vector3(3f, 1.02f, 14f),
                    new Vector3(9.1f, 1.02f, 14f),
                    new Vector3(-5.5f, 1.02f, 14f),
                    new Vector3(14f, 1.02f, 14f)
                };
        }
    }

    private void InitializeUI()
    {
        uiManager.InitializePartyUI(partyManager.partyMembers);
    }

    private GameObject FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
            GameObject result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}
