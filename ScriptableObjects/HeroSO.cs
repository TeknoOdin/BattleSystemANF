using System.Collections;
using System.Collections.Generic;
using AnfBattleSystem;
using UnityEngine;
using UnityEditor;







[CreateAssetMenu(fileName = "New Hero", menuName = "Heroes", order = 0)]
    public class HeroSO : ScriptableObject
{
    public string heroName;
    public int Level;
    public int Strength;
    public int Arcane;
    public int Dexterity;
    public int Faith;
    public List<BaseAttack> Attacks;
    public AbilitySO Ability1;
    public AbilitySO Ability2;


    public BaseClass BaseStats;
    
   
//stats
public void InitializeHero(GameObject heroGameObject)
    {
        Debug.Log("Initializing Hero: " + heroName);
        BaseStats = heroGameObject.AddComponent<BaseClass>();

        BaseStats.Level = Level;
        BaseStats.baseStrength = Strength;
        BaseStats.baseArcane = Arcane;
        BaseStats.baseDexterity = Dexterity;
        BaseStats.baseFaith = Faith;
        BaseStats.meleeAttacks = Attacks;


    }

}


