using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using AnfBattleSystem;
using static Cinemachine.DocumentationSortingAttribute;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies", order = 0)]

public class EnemySO: ScriptableObject
{
    public string enemyName;
    public int Level;
    public int Strength;
    public int Arcane;
    public int Dexterity;
    public int Faith;
    public List<BaseAttack> Attacks;
    public BaseClass BaseStats;

    // Constructor
  

    public void InitializeEnemy(GameObject enemyGameObject)
    {
        // Debug.Log("Initializing Hero: " + heroName);
        BaseStats = enemyGameObject.AddComponent<BaseClass>();
        BaseStats.Level = Level;
        BaseStats.baseStrength = Strength;
        BaseStats.baseArcane = Arcane;
        BaseStats.baseDexterity = Dexterity;
        BaseStats.baseFaith = Faith;
        BaseStats.meleeAttacks = Attacks;
        BaseStats.UpdateStats();
       
      
    }
}