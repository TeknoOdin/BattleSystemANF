using System.Collections;
using System.Collections.Generic;
using AnfBattleSystem;
using UnityEngine;

public class ConsumableItem : Item
{
    public int healAmount;
    public int manaRestore;

    public override void UseItem(BaseHero user)
    {
        user.curHP += healAmount;
        user.curMP += manaRestore;
    }
}