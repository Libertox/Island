using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{

    [CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO/BaseItemSO", order = 0)]
    public class ItemSO : ScriptableObject
    {
        [Header("Base Attributes")]
        public Sprite itemIcon;
        public BaseItem itemPrefab;
        public string itemName;
        public string properties;
        public ItemType itemType;

        public float maxStrength;
        public float useCooldown;
        public int armor;

        [Header("Cooking Attributes")]
        public ItemSO cookFood;
        public float burnSpeed;
        public float cookingTime;

        [Header("Shop Attributes")]
        public int sellValue;
        public int buyValue;

        [Header("Crafting Attributes")]
        public List<ItemSO> needIngredientsToCrafting;
        public List<int> needAmountToCrafting;

        public AudioClip useAudioClip;

        public virtual bool Use(PlayerController player) => false;

        public virtual bool CanUse(PlayerController player) => true;
    }

    [System.Serializable, Flags]
    public enum ItemType
    {
        None = 0,
        Base = 2,
        Single = 4,
        CookedFood = 8,
        Fuel = 16,
        Mineral = 32,
        Blank = 64,
        Backpack = 128,
        Helmet = 256,
        Body = 512,
        Hand = 1024,
        Shoes = 2048,
        Repaired = 4096,
    }
}
