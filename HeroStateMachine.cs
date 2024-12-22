using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AnfBattleSystem;
using System.Linq;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    public enum TurnState
    {
        ADDTOLIST,
        WAITING,
        SELECTING_ACTION,
        PERFORMING_ACTION,
        DEAD
    }

    SimpleAbilitySO SimpleAbilitySO;
    ComplexAbilitySO ComplexAbilitySO;
    public GameObject Selector;
    public HandleTurn handleTurn;
    public BaseHero hero;
    private Image ProgressBar;
    private BaseEnemy enemy;
    private HeroUIManager HGUI;
    private BattleStateMachine BSM;
    private EnemyStateMachine ESM;
    public UIManager GUI;
    private Vector3 startPosition;
    private bool actionSelected = false;
    private bool alive = true;
    public GameObject enemyToAttack;
    private HandleTurn heroTurn;
    // Create a public property to access heroT
    public HandleTurn HeroTurn
    {
        get { return heroTurn; }
        set { heroTurn = value; }
    }
    // Events
    public UnityEvent HeroTurnStartedEvent = new UnityEvent();
    public UnityEvent<GameObject> HeroActionSelectedEvent = new UnityEvent<GameObject>();
    public UnityEvent HeroActionCompletedEvent = new UnityEvent();
    public TurnState currentState;

    private void Awake()
    {
        AssignGUI();
        BSM = GameObject.Find("BattleManager")?.GetComponent<BattleStateMachine>();
        ESM = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyStateMachine>();
        

        SimpleAbilitySO = Resources.Load<SimpleAbilitySO>("Path/To/SimpleAbilitySO");
        ComplexAbilitySO = Resources.Load<ComplexAbilitySO>("Path/To/ComplexAbilitySO");
    }

    private void Start()
    {
        ProgressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
        startPosition = transform.position;
        Selector.SetActive(false);
        currentState = TurnState.WAITING;
        HGUI = GameObject.Find("HeroBar(Clone)")?.GetComponent<HeroUIManager>();
        if (HGUI == null)
        {
            Debug.LogError("HeroUIManager not found!");
        }
        Selector = GameObject.Find("Selector");

        if (GUI == null || BSM == null)
        {
            Debug.LogError("UIManager, BattleStateMachine, or TurnManager not found.");
            enabled = false;
            return;
        }
       // handleTurn = new HandleTurn();
    }

    private void Update()
    {
        switch (currentState)
        {
            case TurnState.ADDTOLIST:
                Debug.Log("Adding hero to manage list");
                this.GUI.heroesToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;

            case TurnState.WAITING:
                break;

            case TurnState.SELECTING_ACTION:
                break;

            case TurnState.PERFORMING_ACTION:
                PerformAction();
                break;

            case TurnState.DEAD:
                if (!alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = "DeadHero";
                    // Handle death logic (similar to original script)
                    Selector.SetActive(false);
                }
                break;
        }
    }

    private void SelectAction()
    {
        Selector.SetActive(true);
        if (actionSelected)
        {
            HeroActionSelectedEvent.Invoke(handleTurn.AttackTarget);
            currentState = TurnState.WAITING;
        }
    }

    private void HandleDeath()
    {
        if (!alive)
        {
            return;
        }
        else
        {
            this.gameObject.tag = "DeadEnemy";
        }
    }

    public void HandleHeroTurnStarted()
    {
        Debug.Log($"Hero turn started for: {hero.heroSO.heroName}");
        currentState = TurnState.ADDTOLIST;
    }

    public enum ActionType
    {
        Attack,
        Item,
        Ability1,
        Ability2
    }

    public ActionType selectedAction;



    public void OnAttackButtonClicked()
    {
        Debug.Log("this in OnAttackButtonClicked(): " + this);

        if (this == null)
        {
            Debug.LogError("this is null in OnAttackButtonClicked()");
            return;
        }

     
       

        AssignGUI();
        selectedAction = ActionType.Attack;
        this.GUI.enemySelectPanel.SetActive(true);
        this.GUI.actionPanel.SetActive(false);
        Debug.Log("Attack button clicked: enemySelectPanel set to active, actionPanel set to inactive");

        // Use the updated variable name
        if (heroTurn == null)
        {
            heroTurn = new HandleTurn(this.gameObject)            {
                Attacker = this.hero.heroSO.name,
                Type = "Hero"
            };
        }
    }

    public void OnAbility1ButtonClicked()
    {
        AssignGUI();

        selectedAction = ActionType.Ability1;
        if (hero.ability1 == SimpleAbilitySO)
        {
            this.GUI.enemySelectPanel.SetActive(true);
            this.GUI.actionPanel.SetActive(false);
            Debug.Log("Ability1 button clicked: enemySelectPanel set to active, actionPanel set to inactive");
           
            heroTurn = new HandleTurn(this.gameObject)
            {
                Attacker = this.hero.heroSO.name,
                AttackerGameObject = this.gameObject,
                Type = "Hero"
            };
        }
        else if (hero.ability1 == ComplexAbilitySO)
        {
            this.GUI.ability2Panel.SetActive(true);
            this.GUI.actionPanel.SetActive(false);
            Debug.Log("Ability1 button clicked: ability2Panel set to active, actionPanel set to inactive");
        }
    }

    public void OnAbility2ButtonClicked()
    {
        AssignGUI();
        if (hero.ability2 == SimpleAbilitySO)
        {
            this.GUI.enemySelectPanel.SetActive(true);
            this.GUI.actionPanel.SetActive(false);
            Debug.Log("Ability1 button clicked: enemySelectPanel set to active, actionPanel set to inactive");

           heroTurn = new HandleTurn(this.gameObject)
           {
                Attacker = this.hero.heroSO.name,
                AttackerGameObject = this.gameObject,
                Type = "Hero"
            };
        }
        else if (hero.ability2 == ComplexAbilitySO)
        {
            this.GUI.ability2Panel.SetActive(true);
            this.GUI.actionPanel.SetActive(false);
            Debug.Log("Ability1 button clicked: ability2Panel set to active, actionPanel set to inactive");
        } 
    }

    public void PerformAction()
    {
        Debug.Log("PerformAction called");
        Debug.Log($"Selected action: {selectedAction}");
        Debug.Log($"Hero: {hero}");
        Debug.Log($"HeroSO: {hero?.heroSO}");
        Debug.Log($"HandleTurn: {handleTurn}");
        Debug.Log($"HeroTurn: {heroTurn}");


        switch (selectedAction)
        {
            case ActionType.Attack:
                if (this.hero.heroSO.Attacks != null && this.hero.heroSO.Attacks.Count > 0)
                {
                    handleTurn.chosenAttack = this.hero.heroSO.Attacks[0];
                    Debug.Log($"Chosen attack: {handleTurn.chosenAttack}");
                    if (handleTurn.chosenAttack != null)
                    {
                        this.hero.Attack(handleTurn.AttackTarget.GetComponent<BaseClass>());
                        Debug.Log(this.hero.heroSO.heroName + " Attacks");
                    }
                }
                else
                {
                    Debug.LogError("No attacks available for this hero.");
                }
                break;
            case ActionType.Item:
                int itemIndex = 0;
                hero.UseItem(itemIndex);
                break;

            case ActionType.Ability1:
                heroTurn.chosenAbility = hero.ability1;
                if (heroTurn.chosenAbility != null)
                {
                    heroTurn.chosenAbility.UseAbility(hero);
                }
                break;
            case ActionType.Ability2:
                heroTurn.chosenAbility = hero.ability2;
                if (heroTurn.chosenAbility != null)
                {
                    heroTurn.chosenAbility.UseAbility(hero);
                }
                break;
        }
        this.GUI.enemySelectPanel.SetActive(false);
        this.GUI.ability2Panel.SetActive(false);

        // Notify TurnManager that the action is completed
        BSM.EndTurn(this.gameObject);
    }

    private void AssignGUI()
    {
        GameObject battleCanvas = GameObject.Find("BattleCanvas");
        if (battleCanvas != null)
        {
            GUI = battleCanvas.GetComponent<UIManager>();
        }
        else
        {
            Debug.LogError("BattleCanvas not found.");
        }
    }
    public void Initialize(BaseHero hero, UIManager uiManager)
    {
        this.hero = hero;
        if (hero == null)
        {
            Debug.LogError("Hero is null in Initialize()");
        }
        this.GUI = uiManager;
        hero.HeroGameObject = this.gameObject;
        Debug.Log($"HeroStateMachine initialized with hero: {hero.heroSO.heroName}");
    }
}