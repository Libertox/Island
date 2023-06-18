using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class Chest : InteractItem
    {
        public static event EventHandler OnInteract;

        private const string ANIM_OPEN_CHEST = "OpenChest";
        private const string IT_IS_CLOSE = "It it close";

        [SerializeField] private ItemInstance[] itemsInChest = new ItemInstance[21];
        [SerializeField] private int[] itemsAmountInChest = new int[21];
        [SerializeField] private bool isLock;
        [SerializeField] private Animator animator;

        private readonly int chestSize = 21;

        public ItemInstance[] ItemInChest => itemsInChest;
        public int[] ItemsAmountInChest => itemsAmountInChest;


        private void Start()
        {
            for (int i = 0; i < chestSize; i++)
                itemsInChest[i] = SaveManager.Instance.CreateLoadedItem(itemsInChest[i]);
        }

        public override void Interact(PlayerController player)
        {
            if (!isLock)
            {
                animator.SetBool(ANIM_OPEN_CHEST, true);
                OnInteract?.Invoke(this, EventArgs.Empty);
            }
            else
                player.SpeechBubble.SetMessage(IT_IS_CLOSE);

        }

        public void SetLock(bool locked) => isLock = locked;

        public bool IsLock() => isLock;

        public override void DestroySelf()
        {
            if (IsLock()) return;

            base.DestroySelf();
            for (int i = 0; i < itemsInChest.Length; i++)
            {
                if (itemsInChest[i] != null)
                {
                    if (!InventoryManager.Instance.AddItem(itemsInChest[i], itemsAmountInChest[i]))
                    {
                        if (itemsInChest[i].itemSO.itemType.HasFlag(ItemType.Single))
                            SpawnBaseItem(itemsInChest[i]);
                        else
                        {
                            for (int j = 0; j < itemsAmountInChest[i]; j++)
                                SpawnBaseItem(itemsInChest[i]);
                        }
                    }

                }
            }

        }

        public void CloseChestAnim() => animator.SetBool(ANIM_OPEN_CHEST, false);

        public void SetItemInChest(ItemInstance itemInstances, int index) => itemsInChest[index] = itemInstances;

        public void SetItemAmountInChest(int amount, int index) => itemsAmountInChest[index] = amount;

        public override ObjectSerializable CreateItemData() => new ChestSerializable(this);
    }

    [System.Serializable]
    public class ChestSerializable : ObjectSerializable
    {
        public ItemInstance[] itemsInChest;
        public int[] itemsAmountInChest;
        public bool isLock;

        public ChestSerializable(Chest chest) : base(chest.transform.position)
        {
            itemsInChest = chest.ItemInChest;
            itemsAmountInChest = chest.ItemsAmountInChest;
            isLock = chest.IsLock();
        }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Chest chest = UnityEngine.Object.Instantiate(saveSystem.PrefabList.chestPrefab);
            chest.transform.position = new Vector3(position.x, position.y, position.z);
            chest.SetLock(isLock);
            for (int i = 0; i < itemsInChest.Length; i++)
            {
                chest.SetItemAmountInChest(itemsAmountInChest[i], i);
                itemsInChest[i] = saveSystem.CreateLoadedItem(itemsInChest[i]);
                chest.SetItemInChest(itemsInChest[i], i);
            }
            return chest;
        }
    }
}
