using AnfBattleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public int strVal;
    public int arcVal;
    public int dexVal;
    public int faithVal;
    public override void UseItem(BaseHero user)
    {
        user.armourValue += arcVal;
        user.strBonus += strVal;
        user.dexBonus += dexVal;
        user.faithBonus += faithVal;
        // Equip the item to the character
    }
}
