using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    [System.Serializable]
    public class BackpackInstance : ItemInstance
    {
        public ItemInstance[] content = new ItemInstance[28];
        public int[] amount = new int[28];

        public BackpackInstance(ItemSO itemSO, float strength, ItemInstance[] content, int[] amount) : base(itemSO, strength)
        {
            this.content = content;
            this.amount = amount;
        }

    }
}

