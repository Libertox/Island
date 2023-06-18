using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.Item;

namespace Island.UI
{

    public class InventoryButtonUI : MonoBehaviour
    {
        [SerializeField] private Button inventoryButton;
        [SerializeField] private InventorySingleUI inventorySingleUI;
        [SerializeField] private List<ItemType> buttonType;

        public ItemInstance Item { get; private set; }
        public int Amount { get; private set; }


        private void Awake() => inventoryButton = GetComponent<Button>();

        private void Start() => inventoryButton.onClick.AddListener(() => GetItemFromCursor());

        private void GetItemFromCursor()
        {
            if (CursorUI.Instance.ItemCursor?.itemSO == null || CheckRightItemType(CursorUI.Instance.ItemCursor.itemSO))
            {
                if (Item?.itemSO != null && Item?.itemSO == CursorUI.Instance.ItemCursor?.itemSO && !Item.itemSO.itemType.HasFlag(ItemType.Backpack))
                {
                    Amount += CursorUI.Instance.ItemAmount;
                    CursorUI.Instance.SwapItem(null);
                    CursorUI.Instance.SwapAmountItem(0);
                }
                else
                {
                    Item = CursorUI.Instance.SwapItem(Item);
                    Amount = CursorUI.Instance.SwapAmountItem(Amount);
                }

                inventorySingleUI.UpdateVisual(Item, Amount);
            }
        }

        private bool CheckRightItemType(ItemSO itemSO)
        {
            foreach (ItemType itemType in buttonType)
            {
                if (itemSO.itemType.HasFlag(itemType))
                    return true;
            }
            return false;
        }

        public void Initialize(ItemInstance itemInstance, int amount)
        {
            Item = itemInstance;
            Amount = amount;
            inventorySingleUI.UpdateVisual(itemInstance, amount);
        }

        public void AddAdditionalListner(UnityEngine.Events.UnityAction call) => inventoryButton.onClick.AddListener(call);

    }
}
