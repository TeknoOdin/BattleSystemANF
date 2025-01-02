using System.Collections;
using System.Collections.Generic;
using AnfBattleSystem;
using UnityEngine;



    [CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        public Sprite itemIcon;
        public ItemType itemType;
        public int forgeValue;
    public enum ItemType
        {
            Consumable,
            Equipment,
            KeyItem
        }

        public virtual void UseItem(BaseHero user)
        {
            // Default behavior: can be overridden by specific item types
            Debug.Log("Using generic item: " + itemName);
        }
    }
