using System.Collections;
using System.Collections.Generic;
using AnfBattleSystem;
using UnityEngine;

public class KeyItem : Item
{
    public string questTrigger;

    public override void UseItem(BaseHero user)
    {
        // Trigger a specific quest or event
        Debug.Log("Using key item: " + questTrigger);
    }
}
