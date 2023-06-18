using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;
using Island.UI;

namespace Island
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        public event EventHandler<OnInventoryChangedEventArgs> OnInventoryChanged;
        public class OnInventoryChangedEventArgs : EventArgs { public int changedItemIndex; }


        [SerializeField] private ItemInstance[] itemsHeld;
        [SerializeField] private int[] numberOfItems;

        private float useItemTime;
        private readonly int looseItemAfterDead = 2;

        public ItemInstance[] ItemsHeld => itemsHeld;
        public int[] NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
        public Armor Armor { get; private set; }
        public ItemInstance UsedItem { get; private set; }
        public int UsedItemIndex { get; private set; }
        public int InventorySize { get; private set; } = 7;
        public bool ItemIsUsed { get; private set; }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            GameInput.Instance.OnItemSelected += GameInput_OnItemSelected;
            GameInput.Instance.OnItemDroped += GameInput_OnItemDroped;
            GameInput.Instance.OnItemUsed += GameInput_OnItemUsed;

            PlayerController.Instance.PlayerStats.OnDied += PlayerStats_OnDied;

            DontDestroyOnLoad(gameObject);
        }

        private void PlayerStats_OnDied(object sender, EventArgs e) => LossInventoryAfterDead();

        private void GameInput_OnItemUsed(object sender, GameInput.OnItemUsedEventArgs e)
        {
            if (!UIManager.Instance.IsNoneState()) return;

            ItemIsUsed = e.isUsed;
        }

        private void GameInput_OnItemDroped(object sender, EventArgs e)
        {
            if (!UIManager.Instance.IsNoneState()) return;

            DropItem();
        }

        private void GameInput_OnItemSelected(object sender, GameInput.OnItemSelectedEventArgs e)
        {
            if (!UIManager.Instance.IsNoneState()) return;

            UsedItemIndex = e.selectedItem - 1;
            UsedItem = ItemsHeld[UsedItemIndex];
        }

        private void Update()
        {
            if (ItemIsUsed)
                UseItem();
        }

        public void InventoryChangedAtIndex(int index)
        {
            OnInventoryChanged?.Invoke(this, new OnInventoryChangedEventArgs
            {
                changedItemIndex = index
            });
        }

        public bool AddItem(ItemInstance itemInstance, int amount = 1)
        {
            if (itemInstance?.itemSO == null)
                return false;

            for (int i = 0; i < ItemsHeld.Length; i++)
            {
                if (ItemsHeld[i]?.itemSO != null)
                {
                    if (ItemsHeld[i]?.itemSO == itemInstance.itemSO && !ItemsHeld[i].itemSO.itemType.HasFlag(ItemType.Single))
                    {
                        NumberOfItems[i] += amount;
                        InventoryChangedAtIndex(i);
                        return true;
                    }
                }
            }

            for (int i = 0; i < ItemsHeld.Length; i++)
            {
                if (ItemsHeld[i]?.itemSO == null)
                {
                    if (!itemInstance.itemSO.itemType.HasFlag(ItemType.Backpack))
                    {
                        ItemsHeld[i] = itemInstance;
                        NumberOfItems[i] += amount;
                    }
                    else
                    {
                        BackpackInstance backpackInstance = itemInstance as BackpackInstance;
                        ItemsHeld[i] = new BackpackInstance(backpackInstance.itemSO, backpackInstance.strength, backpackInstance.content, backpackInstance.amount);
                        NumberOfItems[i] = 1;
                    }

                    if (UsedItem == null)
                    {
                        UsedItem = ItemsHeld[i];
                        UsedItemIndex = i;
                    }
                    InventoryChangedAtIndex(i);
                    return true;
                }
            }
            return false;
        }

        private void DropItem()
        {
            if (UsedItem?.itemSO == null)
                return;

            if (UsedItem.itemSO.itemType.HasFlag(ItemType.Backpack))
            {
                Backpack backpack = Instantiate((Backpack)UsedItem.itemSO.itemPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
                BackpackInstance backpackInstance = UsedItem as BackpackInstance;
                backpack.Content = backpackInstance.content;
                backpack.Amount = backpackInstance.amount;
            }
            else
            {
                BaseItem item = Instantiate(UsedItem.itemSO.itemPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
                item.ItemStrength = UsedItem.strength;
            }
            RemoveItemOnIndex(UsedItemIndex, 1);

        }

        public void RemoveItemOnIndex(int index, int removeAmount)
        {

            if (NumberOfItems[index] >= 1)
                NumberOfItems[index] -= removeAmount;

            if (NumberOfItems[index] == 0)
            {
                ItemsHeld[index] = null;
                if (index == UsedItemIndex)
                    UsedItem = null;
            }

            InventoryChangedAtIndex(index);
        }

        private void UseItem()
        {
            if (UsedItem == null)
                return;

            useItemTime += Time.deltaTime;

            if (UsedItem.itemSO.CanUse(PlayerController.Instance))
                AudioManager.Instance.PlaySoundEffect(UsedItem.itemSO.useAudioClip);

            if (useItemTime > UsedItem.itemSO.useCooldown)
            {
                if (UsedItem.itemSO.Use(PlayerController.Instance))
                {
                    if (UsedItem.itemSO.itemType.HasFlag(ItemType.Single))
                    {
                        UsedItem.strength--;
                        InventoryChangedAtIndex(UsedItemIndex);
                        if (UsedItem.strength < 0)
                            RemoveItemOnIndex(UsedItemIndex, 1);
                    }
                    else
                        RemoveItemOnIndex(UsedItemIndex, 1);
                }
                useItemTime = 0;
            }

        }

        public bool FindItemIndex(ItemSO itemSO, out int index)
        {
            for (int i = 0; i < InventorySize; i++)
            {
                if (ItemsHeld[i]?.itemSO == itemSO)
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

        public bool CheckEnoughItem(ItemSO chekcedItem, int checkedAmount, out int indexPosition)
        {
            if (FindItemIndex(chekcedItem, out indexPosition))
            {
                if (NumberOfItems[indexPosition] >= checkedAmount)
                    return true;
            }
            return false;
        }

        public void UpdateInventory(InventoryButtonUI[] inventoryButtonUI, bool updateUI = false)
        {
            for (int i = 0; i < inventoryButtonUI.Length; i++)
            {
                ItemsHeld[i] = inventoryButtonUI[i].Item;
                NumberOfItems[i] = inventoryButtonUI[i].Amount;
                if (updateUI)
                    InventoryChangedAtIndex(i);
            }
            UsedItem = ItemsHeld[UsedItemIndex];
            InventoryChangedAtIndex(UsedItemIndex);
        }

        public void LossInventoryAfterDead()
        {
            for (int i = 0; i < looseItemAfterDead; i++)
            {
                int itemIndex = UnityEngine.Random.Range(0, ItemsHeld.Length);
                if (ItemsHeld[itemIndex]?.itemSO != null)
                {
                    if (ItemsHeld[itemIndex].itemSO.itemType.HasFlag(ItemType.Single))
                        RemoveItemOnIndex(itemIndex, 0);
                    else
                        RemoveItemOnIndex(itemIndex, NumberOfItems[itemIndex] / 2);
                }
            }
            if (Armor.backpack?.itemSO != null)
            {
                int looseBackpackItem = Armor.backpack.content.Length / 2;
                int starteDelete = UnityEngine.Random.Range(0, Armor.backpack.content.Length);
                for (int i = 0; i < looseBackpackItem; i++)
                {
                    int currentIndex = (i + starteDelete) % Armor.backpack.content.Length;
                    Armor.backpack.content[currentIndex] = null;
                    Armor.backpack.amount[currentIndex] = 0;
                }
            }

            UsedItem = null;

        }

        public void SetNewArmor(Armor armor) => Armor = armor;

    }
}