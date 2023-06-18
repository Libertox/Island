using System.Collections;
using UnityEngine;
using Island.Item;

namespace Island
{
    public class SaveInventory : MonoBehaviour
    {
        private const string INVENTORY_ITEM_KEY = "/Inventory_Item";
        private const string INVENTORY_AMOUNT_KEY = "/Inventory_Amount";
        private const string INVENTORY_ARMOR_KEY = "/Inventory_Armor";

        private InventoryManager inventoryManager;

        private void Awake() => inventoryManager = GetComponent<InventoryManager>();


        private void Start()
        {
            SaveManager.Instance.OnSavedGame += SaveManager_OnSavedGame;

            if (!PlayerPrefs.HasKey(SceneLoader.GetCurrentSceneName()))
            {
                for (int i = 0; i < inventoryManager.InventorySize; i++)
                    inventoryManager.ItemsHeld[i] = SaveManager.Instance.CreateLoadedItem(inventoryManager.ItemsHeld[i]);

                Save();
            }
            else
                Load();
        }

        private void SaveManager_OnSavedGame(object sender, System.EventArgs e) => Save();

        private void Save()
        {
            SaveManager.Save(inventoryManager.ItemsHeld, INVENTORY_ITEM_KEY);
            SaveManager.Save(inventoryManager.NumberOfItems, INVENTORY_AMOUNT_KEY);
            SaveManager.Save(inventoryManager.Armor, INVENTORY_ARMOR_KEY);
        }

        private void Load()
        {
            ItemInstance[] itemInstance = SaveManager.Load<ItemInstance[]>(INVENTORY_ITEM_KEY);
            inventoryManager.NumberOfItems = SaveManager.Load<int[]>(INVENTORY_AMOUNT_KEY);
            for (int i = 0; i < inventoryManager.InventorySize; i++)
            {
                inventoryManager.ItemsHeld[i] = SaveManager.Instance.CreateLoadedItem(itemInstance[i]);

                inventoryManager.InventoryChangedAtIndex(i);
            }

            Armor armor = SaveManager.Load<Armor>(INVENTORY_ARMOR_KEY);
            inventoryManager.SetNewArmor(new Armor
            {
                backpack = SaveManager.Instance.CreateLoadedBackpack(armor.backpack),
                helmet = SaveManager.Instance.CreateLoadedItem(armor.helmet),
                body = SaveManager.Instance.CreateLoadedItem(armor.body),
                shoes = SaveManager.Instance.CreateLoadedItem(armor.shoes),
                hand = SaveManager.Instance.CreateLoadedItem(armor.hand)
            });
        }
    }
}