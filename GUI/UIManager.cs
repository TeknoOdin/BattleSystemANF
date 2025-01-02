using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnfBattleSystem;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    private HeroUIManager heroUIManager;
    public GameObject HeroPanel; // Reference to the HeroPanel prefab
    public Transform HeroSpacer; // Reference to the HeroPanelSpacer transform
    TurnManager turnManager;
    private PartyManager partyManager;
    public Transform spacer;
    private BattleStateMachine bsm;
    public GameObject actionPanel;
    public GameObject enemySelectPanel;
    public GameObject ability2Panel;
    public GameObject selector;
    public GameObject enemyButton;
    private HeroStateMachine hsm;
    private EnemyStateMachine esm;
    // Magic attack
    public Transform actionSpacer;
    public Transform ability2Spacer;
    public GameObject actionButton;
    public GameObject ability1Button;
    public GameObject ability2Button;
    public EnemySelectButton enemySelectButton;
    
    private List<GameObject> atkBtns = new List<GameObject>();
    private List<GameObject> enemyBtns = new List<GameObject>();

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        DONE,
    }

    public HeroGUI HeroInput;
    public List<GameObject> heroesToManage = new List<GameObject>();
 

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        esm = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyStateMachine>();
        bsm = GameObject.Find("BattleManager")?.GetComponent<BattleStateMachine>();
        hsm = GameObject.FindGameObjectWithTag("Hero")?.GetComponent<HeroStateMachine>();
        heroUIManager = GameObject.Find("BattleCanvas")?.GetComponent<HeroUIManager>();
        partyManager = FindObjectOfType<PartyManager>();
        HeroInput = HeroGUI.ACTIVATE;
    }

    private void Update()
    {
       
        switch (HeroInput)
        {
            case HeroGUI.ACTIVATE:
                if (heroesToManage.Count > 0)
                {
                    heroesToManage[0].transform.Find("Hero").gameObject.transform.Find("Selector").gameObject.SetActive(true);
                   actionPanel.SetActive(true);
                    // Ensure CreateAttackButtons is only called once per activation
                    CreateAttackButtons();
                    HeroInput = HeroGUI.WAITING;
                }
                break;

            case HeroGUI.WAITING:
                // Waiting for player input
                break;

            case HeroGUI.DONE:
                HeroInputDone();
                break;
        }
    }

    public void InitializePartyUI(List<BaseHero> heroes)
    {
        foreach (BaseHero hero in heroes)
        {
            GameObject newPanel = Instantiate(HeroPanel);
            newPanel.transform.SetParent(HeroSpacer, false);
            HeroUIManager panelManager = newPanel.GetComponent<HeroUIManager>();
            panelManager.CreateHeroPanel(hero);
        }
    }

    public void EnemySelection(GameObject chooseEnemy)
    {
      
        hsm.HeroTurn.AttackTarget = chooseEnemy;
        HeroInput = HeroGUI.DONE;
    }

    public void HeroInputDone()
    {
        bsm.CollectActions(hsm.HeroTurn);
        ClearAttackPanel();

       // heroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        heroesToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    public void ClearAttackPanel()
    {
        enemySelectPanel.SetActive(false);
        actionPanel.SetActive(false);
        ability2Panel.SetActive(false);
        Debug.Log("ClearAttackPanel: enemySelectPanel, actionPanel, and ability2Panel set to inactive");

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    public void EnemyButtons()
    {
        // Cleanup
        foreach (GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();

        // Create buttons
        foreach (GameObject enemy in turnManager.enemiesInBattle)
        {
            
            {
                GameObject newButton = Instantiate(enemyButton);
                EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();
                TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = esm.enemy.enemySO.enemyName;
                button.EnemyPrefab = turnManager.enemiesInBattle[0];

                newButton.transform.SetParent(spacer, false);
                enemyBtns.Add(newButton);
            }
        }
    }

    public void CreateAttackButtons()
    {
        if (heroesToManage.Count == 0)
        {
            Debug.LogError("No hero found in the turn queue.");
            return;
        }

        hsm.hero = heroesToManage[0].GetComponent<HeroStateMachine>().hero;

        CreateButton("Attack", () => hsm.OnAttackButtonClicked(), actionSpacer);
        CreateButton(hsm.hero.heroSO.Ability1.AbilityName, () => hsm.OnAbility1ButtonClicked(), actionSpacer);
        CreateButton(hsm.hero.heroSO.Ability2.AbilityName, () => hsm.OnAbility2ButtonClicked(), actionSpacer);
    }

    private void CreateButton(string buttonText, UnityEngine.Events.UnityAction onClickAction, Transform parent)
    {
        GameObject button = Instantiate(actionButton);
        TextMeshProUGUI buttonTextComponent = button.transform.Find("ActionText").GetComponent<TextMeshProUGUI>();
        if (buttonTextComponent == null)
        {
            Debug.LogError("ActionText not found in button.");
            return;
        }
        buttonTextComponent.text = buttonText;
        button.GetComponent<Button>().onClick.AddListener(onClickAction);
        button.transform.SetParent(parent, false);
        atkBtns.Add(button);
    }
}