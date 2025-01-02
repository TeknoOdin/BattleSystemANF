using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using AnfBattleSystem;
using static UIManager;

public class BattleStateMachine : MonoBehaviour
{
    public PartyManager PartyManager { get; private set; }
    public EncounterManager EncounterManager { get; private set; }
    public BattleManager BattleManager { get; private set; }
    public TurnManager TurnManager { get; private set; }

    public HeroStateMachine HSM { get; private set; }
    public EnemyStateMachine ESM { get; private set; }
    private UIManager GUI;
    private HeroUIManager HGUI;

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }

    public PerformAction battleStates { get; set; }

    public List<HandleTurn> performList = new List<HandleTurn>();
    public List<GameObject> heroesInGame = new List<GameObject>();
    public List<GameObject> enemiesInBattle = new List<GameObject>();

    private void Awake()
    {
        CacheReferences();
        heroesInGame.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        enemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    private void CacheReferences()
    {
        PartyManager = FindObjectOfType<PartyManager>();
        EncounterManager = FindObjectOfType<EncounterManager>();
        BattleManager = FindObjectOfType<BattleManager>();
        TurnManager = FindObjectOfType<TurnManager>();
        GUI = GameObject.Find("BattleCanvas").GetComponent<UIManager>();
        HGUI = GameObject.Find("EventSystem").GetComponentInChildren<HeroUIManager>();

        if (PartyManager == null || EncounterManager == null || BattleManager == null || TurnManager == null)
        {
            Debug.LogError("One or more managers are not found!");
        }
    }

    private void Start()
    {
        InitializeStateMachines();
        InitializeGUI();
        battleStates = PerformAction.WAIT;
        TurnManager.InitializeTurnOrder();
        TurnManager.StartTurn();
    }

    private void Update()
    {
        switch (battleStates)
        {
            case PerformAction.WAIT:
                if (performList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                break;

            case PerformAction.TAKEACTION:
                if (performList.Count > 0 && performList[0] != null)
                {
                    GameObject performer = performList[0].AttackerGameObject;

                    if (performer != null)
                    {
                        if (performList[0].Type == "Hero")
                        {
                            HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                            HSM.enemyToAttack = performList[0].AttackTarget;
                            HSM.currentState = HeroStateMachine.TurnState.PERFORMING_ACTION;

                        }
                        else if (performList[0].Type == "Enemy")
                        {
                            EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                            ESM.heroToAttack = performList[0].AttackTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.PERFORMING_ACTION;
                        }
                        battleStates = PerformAction.PERFORMACTION;
                    }
                    else
                    {
                        Debug.LogError("Performer not found!");
                    }
                }
                break;

            case PerformAction.PERFORMACTION:
                // Perform the action and then transition back to WAIT
                // Assuming actions are performed immediately for simplicity
                break;

            case PerformAction.CHECKALIVE:
                // Check if all characters are alive
                // If not, transition to LOSE
                // If all enemies are dead, transition to WIN
                break;

            case PerformAction.WIN:
                break;

            case PerformAction.LOSE:
                break;
        }
    }

    private void InitializeStateMachines()
    {
        HSM = GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStateMachine>();
        ESM = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStateMachine>();

        if (HSM == null) Debug.LogError("HeroStateMachine is not found!");
        if (ESM == null) Debug.LogError("EnemyStateMachine is not found!");
    }

    private void InitializeGUI()
    {
        GUI.actionPanel.SetActive(false);
        GUI.ability2Panel.SetActive(false);
        GUI.enemySelectPanel.SetActive(false);
        GUI.HeroInput = UIManager.HeroGUI.ACTIVATE;
        GUI.EnemyButtons();
        Debug.Log("InitializeGUI: actionPanel, ability2Panel, and enemySelectPanel set to inactive");
    }

    public void CollectActions(HandleTurn input)
    {
        performList.Add(input);
    }

    public void EndTurn(GameObject participant)
    {
        TurnManager.EndTurn(participant);
    }
}