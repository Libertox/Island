using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{
    public class AnvilUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button repairButton;

        [SerializeField] private Image spaceIcon;

        [SerializeField] private Image[] ingredients;
        [SerializeField] private TextMeshProUGUI[] ingredientsText;

        [SerializeField] private InventoryButtonUI repairedItemButton;
        [SerializeField] private InventoryButtonUI[] playerInventoryButtons;

        [SerializeField] private UIManager UIManager;

        private Anvil chooseAnvil;

        private void Start()
        {
            Anvil.OnInteract += Anvil_OnInteract;

            repairedItemButton.AddAdditionalListner(() => UpdateNeedIngredients());

            repairButton.onClick.AddListener(() =>
            {
                RepairItem();
                AudioManager.Instance.PlayButtonSoundEffect();
            });

            closeButton.onClick.AddListener(() =>
            {
                if (CursorUI.Instance.ItemCursor == null)
                {
                    AudioManager.Instance.PlayButtonSoundEffect();
                    chooseAnvil.RepairedItem = repairedItemButton.Item;
                    InventoryManager.Instance.UpdateInventory(playerInventoryButtons, true);
                    CursorUI.Instance.Hide();
                    Hide();
                    UIManager.SetGameState(UIOpenState.None);
                }
            });

            Hide();
        }

        private void Anvil_OnInteract(object sender, System.EventArgs e)
        {
            chooseAnvil = sender as Anvil;
            repairedItemButton.Initialize(chooseAnvil.RepairedItem, 0);
            UIManager.SetGameState(UIOpenState.ItemUI);
            Show();
            for (int i = 0; i < playerInventoryButtons.Length; i++)
            {
                playerInventoryButtons[i].Initialize(InventoryManager.Instance.ItemsHeld[i], InventoryManager.Instance.NumberOfItems[i]);
            }
            UpdateNeedIngredients();
        }

        private void OnDestroy() => Anvil.OnInteract -= Anvil_OnInteract;

        private void UpdateNeedIngredients()
        {
            if (repairedItemButton.Item != null)
            {
                spaceIcon.gameObject.SetActive(false);

                for (int i = 0; i < ingredients.Length; i++)
                {
                    ingredients[i].gameObject.SetActive(false);
                    ingredientsText[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < repairedItemButton.Item.itemSO.needIngredientsToCrafting.Count; i++)
                {
                    ingredients[i].gameObject.SetActive(true);
                    ingredients[i].sprite = repairedItemButton.Item.itemSO.needIngredientsToCrafting[i].itemIcon;

                    ingredientsText[i].gameObject.SetActive(true);
                    if (InventoryManager.Instance.FindItemIndex(repairedItemButton.Item.itemSO.needIngredientsToCrafting[i], out int index))
                        ingredientsText[i].SetText($"{InventoryManager.Instance.NumberOfItems[index]} / {(int)(repairedItemButton.Item.itemSO.needAmountToCrafting[i] * 0.5f)}");
                    else
                        ingredientsText[i].SetText($"0 / {(int)(repairedItemButton.Item.itemSO.needAmountToCrafting[i] * 0.5f)}");
                }
            }
            else
            {
                spaceIcon.gameObject.SetActive(true);

                for (int i = 0; i < ingredients.Length; i++)
                {
                    ingredients[i].gameObject.SetActive(false);
                    ingredientsText[i].gameObject.SetActive(false);
                }
            }
        }

        private void RepairItem()
        {
            if (repairedItemButton.Item != null && CursorUI.Instance.ItemCursor == null)
            {
                InventoryManager.Instance.UpdateInventory(playerInventoryButtons);
                bool isEnough = false;
                int[] index = new int[repairedItemButton.Item.itemSO.needIngredientsToCrafting.Count];

                for (int i = 0; i < repairedItemButton.Item.itemSO.needIngredientsToCrafting.Count; i++)
                    isEnough = InventoryManager.Instance.CheckEnoughItem(repairedItemButton.Item.itemSO.needIngredientsToCrafting[i], (int)(repairedItemButton.Item.itemSO.needAmountToCrafting[i] * 0.5f), out index[i]);

                if (isEnough)
                {
                    for (int i = 0; i < index.Length; i++)
                    {
                        InventoryManager.Instance.RemoveItemOnIndex(index[i], (int)(repairedItemButton.Item.itemSO.needAmountToCrafting[i] * 0.5f));
                        playerInventoryButtons[index[i]].Initialize(InventoryManager.Instance.ItemsHeld[index[i]], InventoryManager.Instance.NumberOfItems[index[i]]);
                    }
                    repairedItemButton.Item.SetMaxStrength();
                    repairedItemButton.Initialize(repairedItemButton.Item, 0);
                    UpdateNeedIngredients();
                }
            }
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
