
using System;
using System.Collections.Generic;
using UnityEngine;
using AnfBattleSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Events;

public class PartyManager : MonoBehaviour
{
    [SerializeField] public List<BaseHero> partyMembers = new List<BaseHero>();
    [SerializeField] public List<HeroSO> heroSOs; // Reference to your HeroSOs

    private int maxPartySize = 4;

    public BaseHero ActiveMember { get; private set; }
    public BattleStateMachine bsm { get; private set; }
    public static PartyManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }
    public UnityAction OnPartyAdded;
    public void InitializeParty()
    {


        for (int i = 0; i < maxPartySize && i < heroSOs.Count; i++)
        {

            HeroSO heroSO = heroSOs[i];
            GameObject heroObject = new GameObject($"Hero_{i}");
            BaseHero hero = heroObject.AddComponent<BaseHero>();
            hero.InitializeHero(heroSO);
            if (hero.heroSO == null)
            {
                Debug.LogError($"HeroSO is null for hero at index {i}");
            }
            else
            {
                Debug.Log($"HeroSO for hero at index {i} is {hero.heroSO.heroName}");
                if (hero.heroSO.Ability1 == null)
                {
                    Debug.LogError($"Ability1 is null for hero {hero.heroSO.heroName}");
                }
                if (hero.heroSO.Ability2 == null)
                {
                    Debug.LogError($"Ability2 is null for hero {hero.heroSO.heroName}");
                }
            }
            partyMembers.Add(hero);

        }

    }



    public void AddToParty(BaseHero member)
    {
        if (partyMembers.Count < maxPartySize)
        {
            partyMembers.Add(member);
            if (partyMembers.Count == 1)
            {
                SetActiveMember(member);
            }
        }
        else
        {
            Debug.Log("Party is full!");
        }

    }

    public void RemoveFromParty(BaseHero member)
    {
        partyMembers.Remove(member);
        if (partyMembers.Count > 0)
        {
            SetActiveMember(partyMembers[0]);
        }
        else
        {
            ActiveMember = null;
        }
    }

    public void SetActiveMember(BaseHero member)
    {
        ActiveMember = member;
        member.UpdateStats();


    }
}
