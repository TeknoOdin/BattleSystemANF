
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using AnfBattleSystem;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;


public class HeroUIManager : MonoBehaviour
{
    [SerializeField] public TMP_Text HeroName;
    [SerializeField] public TMP_Text hpText;
    [SerializeField] public TMP_Text mpText;

    [SerializeField] public Image AssendBar;
    [SerializeField] public Image ProgressBar;

    public GameObject HeroPanel;
    public Transform HeroSpacer;

     // Reference to the current hero
    public GameObject enemyButton;
    public Transform spacer;
    private BattleStateMachine bsm;
    public GameObject actionPanel;
    public GameObject enemySelectPanel;
    public GameObject abilityPanel;
    HeroStateMachine hsm;
    //magic attack
    public Transform actionSpacer;
    public Transform abilitySpacer;
    public GameObject actionButton;
    public GameObject ability1Button;
    public GameObject ability2Button;
    private List<GameObject> atkBtns = new List<GameObject>();

    // enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();
    public void CreateHeroPanel(BaseHero hero)
    {
        if (hero != null && hero.heroSO != null)
        {
            // Update UI elements
            HeroName.text = hero.heroSO.heroName;
            // ... other UI updates
        }
        else
        {
            Debug.LogError("Hero or HeroSO is null");
        }
        int roundedHP = Mathf.RoundToInt(hero.curHP);
        int roundedMaxHP = Mathf.RoundToInt(hero.maxHP);
        int roundedMP = Mathf.RoundToInt(hero.curMP);
        int roundedMaxMP = Mathf.RoundToInt(hero.maxMP);
        hpText.text = $" {roundedHP}/{roundedMaxHP}";
        mpText.text = $" {roundedMP}/{roundedMaxMP}";
       


        hero.OnStatsUpdated += UpdateHeroPanel;
        Debug.Log("subscribed to OnstatsUpdated");
    }

      
    


    public void UpdateHeroPanel()
    {
        // Update UI elements directly
        int roundedHP = Mathf.RoundToInt(hsm.hero.curHP);
        int roundedMaxHP = Mathf.RoundToInt(hsm.hero.maxHP);
        int roundedMP = Mathf.RoundToInt(hsm.hero.curMP);
        int roundedMaxMP = Mathf.RoundToInt(hsm.hero.maxMP);

        hpText.text = $"{roundedHP}/{roundedMaxHP}";
        mpText.text = $"{roundedMP}/{roundedMaxMP}";
    }
   
    }