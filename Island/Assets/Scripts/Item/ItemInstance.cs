using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{

    [System.Serializable]
    public class ItemInstance
    {
        [System.NonSerialized]
        public ItemSO itemSO;
        public float strength;
        public string itemSOName;

        public ItemInstance(ItemSO itemSO, float strength)
        {
            this.itemSO = itemSO;
            this.strength = strength;
            this.itemSOName = itemSO?.name;
        }

        public void SetMaxStrength() => strength = itemSO.maxStrength;

    }
}

