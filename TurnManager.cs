using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AnfBattleSystem;
using NUnit.Framework;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> heroesInBattle = new List<GameObject>();
    public List<GameObject> enemiesInBattle = new List<GameObject>();
    public List<GameObject> turnOrder = new List<GameObject>();
    PartyManager partyManager;
    EncounterManager encounterManager;
    public static TurnManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitializeTurnOrder();
        StartTurn();
    }

    public void InitializeTurnOrder()
    {
        // Combine heroes and enemies into a single list of participants
        List<GameObject> allParticipants = new List<GameObject>();
        allParticipants.AddRange(heroesInBattle);
        allParticipants.AddRange(enemiesInBattle);

        // Clear the current turn order
        turnOrder.Clear();

        // Calculate initiative for each participant and add them to the turn order
        foreach (var participant in allParticipants)
        {
            HeroStateMachine HSM = participant.GetComponent<HeroStateMachine>();
            EnemyStateMachine ESM = participant.GetComponent<EnemyStateMachine>();

            if (HSM != null)
            {
                HSM.hero.CalculateInitiative();
                turnOrder.Add(participant);
                Debug.Log($"Added {participant.name} (Hero) with initiative {HSM.hero.Initiative} to turn order.");
            }
            else if (ESM != null)
            {
                ESM.enemy.CalculateInitiative();
                turnOrder.Add(participant);
                Debug.Log($"Added {participant.name} (Enemy) with initiative {ESM.enemy.Initiative} to turn order.");
            }
            else
            {
                Debug.LogWarning($"Participant {participant.name} does not have a BaseHero or BaseEnemy component.");
            }
        }

        // Sort the turn order based on the initiative values in descending order
        turnOrder.Sort((a, b) =>
        {
            var aInitiative = a.GetComponent<HeroStateMachine>()?.hero.Initiative ?? a.GetComponent<EnemyStateMachine>().enemy.Initiative;
            var bInitiative = b.GetComponent<HeroStateMachine>()?.hero.Initiative ?? b.GetComponent<EnemyStateMachine>().enemy.Initiative;
            return bInitiative.CompareTo(aInitiative);
        });

        Debug.Log("Turn order initialized.");
    }

    public void StartTurn()
    {
        if (turnOrder.Count == 0)
        {
            InitializeTurnOrder();
        }

        GameObject currentParticipant = turnOrder[0];
        turnOrder.RemoveAt(0);

        Debug.Log($"Starting turn for {currentParticipant.name}.");

        // Check if the current participant is a BaseHero or BaseEnemy
        HeroStateMachine heroStateMachine = currentParticipant.GetComponent<HeroStateMachine>();
        EnemyStateMachine enemyStateMachine = currentParticipant.GetComponent<EnemyStateMachine>();

        if (heroStateMachine != null)
        {
            Debug.Log($"Current participant is a hero: {heroStateMachine.hero.heroSO.heroName}");
            heroStateMachine.HandleHeroTurnStarted();
            Debug.Log(currentParticipant.name + " Turn Started");
        }
        else if (enemyStateMachine != null)
        {
            enemyStateMachine.HandleEnemyTurnStarted();
            Debug.Log(currentParticipant.name + " Turn Started");
        }
        else
        {
            Debug.LogWarning($"Current participant {currentParticipant.name} is neither a BaseHero nor a BaseEnemy.");
        }
    }

    public void EndTurn(GameObject participant)
    {
        turnOrder.Add(participant);
        Debug.Log($"Ending turn for {participant.name}. Starting next turn.");
        StartTurn();
    }
}
