using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySingleUI inventorySingleUIPrefab;
        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private Transform usedItemIndicator;

        private List<InventorySingleUI> inventorySingleUIList = new List<InventorySingleUI>();

        private InventoryManager inventoryManager;

        private void Start()
        {
            InventoryManager.Instance.OnInventoryChanged += GameManager_OnItemAdded;
            GameInput.Instance.OnItemSelected += GameInput_OnItemSelected;

            inventoryManager = InventoryManager.Instance;
            usedItemIndicator.gameObject.SetActive(false);

            CreateInventorySlots();
        }

        private void GameInput_OnItemSelected(object sender, GameInput.OnItemSelectedEventArgs e) => UpdateUseItemIndicator(e.selectedItem - 1);

        private void GameManager_OnItemAdded(object sender, InventoryManager.OnInventoryChangedEventArgs e)
        {
            inventorySingleUIList[e.changedItemIndex].UpdateVisual(inventoryManager.ItemsHeld[e.changedItemIndex], inventoryManager.NumberOfItems[e.changedItemIndex]);
            UpdateUseItemIndicator(inventoryManager.UsedItemIndex);
        }

        private void CreateInventorySlots()
        {
            for (int i = 0; i < inventoryManager.InventorySize; i++)
            {
                inventorySingleUIList.Add(Instantiate(inventorySingleUIPrefab, inventoryContainer));
                inventorySingleUIList[i].UpdateVisual(inventoryManager.ItemsHeld[i], inventoryManager.NumberOfItems[i]);
            }
        }

        private void UpdateUseItemIndicator(int positionIndex)
        {
            if (inventoryManager.UsedItem == null)
                usedItemIndicator.gameObject.SetActive(false);
            else
            {
                usedItemIndicator.transform.position = inventorySingleUIList[positionIndex].transform.position;
                usedItemIndicator.gameObject.SetActive(true);
            }
        }

    }
}
