using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    public class Backpack : BaseItem
    {
        private ItemInstance[] content = new ItemInstance[28];
        private int[] amount = new int[28];

        public ItemInstance[] Content { get => content; set => content = value; }
        public int[] Amount { get => amount; set => amount = value; }

        public override void Interact(PlayerController player)
        {
            if (InventoryManager.Instance.AddItem(new BackpackInstance(ItemSO, ItemStrength, Content, Amount)))
                DestroySelf();
        }
    }
}
