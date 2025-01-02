using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AnfBattleSystem;
using System.Linq;

public class EnemyStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    private HeroStateMachine HSM;
    public BaseEnemy enemy;

    public enum TurnState
    {
        WAITING,
        SELECTING_ACTION,
        PERFORMING_ACTION,
        DEAD
    }

    public TurnState currentState;

    private Vector3 startPosition;
    public GameObject Selector;
    private bool actionStarted = false;
    public GameObject heroToAttack;
    private float animSpeed = 10f;
    public UnityEvent EnemyTurnStartedEvent;
    public UnityEvent<GameObject> EnemyActionSelectedEvent;
    public UnityEvent EnemyActionCompletedEvent;
    public UnityEvent OnEnemyActionSelectionComplete; // Add this event

    private bool alive = true;

    private void Awake()
    {
        BSM = GameObject.Find("BattleManager")?.GetComponent<BattleStateMachine>();
        HSM = GameObject.FindGameObjectWithTag("Hero")?.GetComponent<HeroStateMachine>();
        if (BSM == null || HSM == null)
        {
            Debug.LogError("BattleStateMachine or HeroStateMachine not found.");
            enabled = false;
            return;
        }

        EnemyActionSelectedEvent = new UnityEvent<GameObject>();
        EnemyActionCompletedEvent = new UnityEvent();
    }

    private void Start()
    {
        startPosition = transform.position;
        currentState = TurnState.WAITING;
    }

    private void Update()
    {
        switch (currentState)
        {
            case TurnState.WAITING:
                break;

            case TurnState.SELECTING_ACTION:
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case TurnState.PERFORMING_ACTION:
                StartCoroutine(TimeForAction());
                break;

            case TurnState.DEAD:
                if (!alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = "DeadEnemy";
                    // Handle death logic (similar to original script)
                    Selector.SetActive(false);
                }
                break;
        }
    }

    public void HandleEnemyTurnStarted()
    {
        currentState = TurnState.SELECTING_ACTION;
    }

    public void ChooseAction()
    {
        var heroes = TurnManager.instance.heroesInBattle;

        heroToAttack = heroes[Random.Range(0, heroes.Count)];
        if (heroToAttack == null)
        {
            Debug.LogError("No valid hero target found.");
            return;
        }
        if (enemy.meleeAttacks == null || enemy.meleeAttacks.Count == 0)
        {
            Debug.LogError("No melee attacks available for this enemy.");
            return;
        }

        HandleTurn enemyTurn = new HandleTurn(this.gameObject)
        {
            Attacker = this.enemy.enemySO.enemyName,
            AttackerGameObject = this.gameObject,
            Type = "Enemy",
            AttackTarget = heroToAttack,
            chosenAttack = enemy.meleeAttacks[Random.Range(0, enemy.meleeAttacks.Count)]
        };

        BSM.CollectActions(enemyTurn);
        Debug.Log($"{gameObject.name} has chosen {enemyTurn.chosenAttack.attackName}");
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
            yield break;

        actionStarted = true;
        heroToAttack = BSM.performList[0].AttackTarget;
        Vector3 heroPosition = new Vector3(heroToAttack.transform.position.x, heroToAttack.transform.position.y, heroToAttack.transform.position.z + 2f);
        while (MoveTowards(heroPosition)) { yield return null; }

        yield return new WaitForSeconds(1f);

        // Move back to start position
        while (MoveTowards(startPosition)) { yield return null; }

        BSM.performList.RemoveAt(0);
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

        actionStarted = false;
        currentState = TurnState.WAITING;

        // Notify TurnManager that the action is completed
        BSM.EndTurn(this.gameObject);
    }

    private bool MoveTowards(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    public void Initialize(BaseEnemy enemy)
    {
        this.enemy = enemy;
        enemy.enemyGameObject = this.gameObject;
    }
}