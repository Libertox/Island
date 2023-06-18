using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Island.Item;

namespace Island.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private InventoryButtonUI[] playerInventoryButtons;

        [SerializeField] private GameObject backpackBlockedText;
        [SerializeField] private GameObject backpackContent;
        [SerializeField] private InventoryButtonUI[] backbackInventoryButtons;

        [SerializeField] private InventoryButtonUI helmetButton;
        [SerializeField] private InventoryButtonUI bodyButton;
        [SerializeField] private InventoryButtonUI handButton;
        [SerializeField] private InventoryButtonUI shoesButton;
        [SerializeField] private InventoryButtonUI backpackButton;

        [SerializeField] private Image helmetIcon;
        [SerializeField] private Image bodyIcon;
        [SerializeField] private Image handIcon;
        [SerializeField] private Image shoesIcon;
        [SerializeField] private Image backpackIcon;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider tirednessSlider;
        [SerializeField] private Slider hungrySlider;

        [SerializeField] private TextMeshProUGUI healthStatusText;
        [SerializeField] private TextMeshProUGUI tirednessStatusText;
        [SerializeField] private TextMeshProUGUI hungryStatusText;

        [SerializeField] private UIManager UIManager;

        private bool isShow;

        private void Start()
        {
            GameInput.Instance.OnPlayerUIOpened += GameInput_OnPlayerUIOpened;

            backpackButton.AddAdditionalListner(() => UpdateBackpackSlots());

            helmetButton.AddAdditionalListner(() => SetupItemIcon(helmetButton, helmetIcon));
            bodyButton.AddAdditionalListner(() => SetupItemIcon(bodyButton, bodyIcon));
            shoesButton.AddAdditionalListner(() => SetupItemIcon(shoesButton, shoesIcon));
            handButton.AddAdditionalListner(() => SetupItemIcon(handButton, handIcon));

            gameObject.SetActive(false);
        }

        private void GameInput_OnPlayerUIOpened(object sender, System.EventArgs e)
        {
            if (!UIManager.Instance.IsNoneState() && !UIManager.Instance.IsInventoryState())
                return;

            if (CursorUI.Instance.ItemCursor?.itemSO == null)
            {
                isShow = !isShow;
                InventoryManager inventoryManager = InventoryManager.Instance;
                PlayerStats playerStats = PlayerController.Instance.PlayerStats;

                if (isShow)
                    Show(inventoryManager, playerStats);
                else
                    Hide(inventoryManager, playerStats);

                gameObject.SetActive(isShow);
            }
        }

        private void Hide(InventoryManager inventoryManager, PlayerStats playerStats)
        {
            UIManager.SetGameState(UIOpenState.None);
            inventoryManager.UpdateInventory(playerInventoryButtons, true);
            inventoryManager.SetNewArmor(new Armor
            {
                body = bodyButton.Item,
                helmet = helmetButton.Item,
                shoes = shoesButton.Item,
                hand = handButton.Item,
                backpack = (BackpackInstance)backpackButton.Item

            });

            if (inventoryManager.Armor.backpack?.itemSO != null)
            {
                for (int i = 0; i < inventoryManager.Armor.backpack.content.Length; i++)
                {
                    inventoryManager.Armor.backpack.content[i] = backbackInventoryButtons[i].Item;
                    inventoryManager.Armor.backpack.amount[i] = backbackInventoryButtons[i].Amount;
                }
            }

            playerStats.UpdateArmor(inventoryManager.Armor);

            CursorUI.Instance.Hide();
        }

        private void Show(InventoryManager inventoryManager, PlayerStats playerStats)
        {
            UIManager.SetGameState(UIOpenState.InventoryUI);

            backpackButton.Initialize(inventoryManager.Armor.backpack, 1);
            helmetButton.Initialize(inventoryManager.Armor.helmet, 0);
            bodyButton.Initialize(inventoryManager.Armor.body, 0);
            shoesButton.Initialize(inventoryManager.Armor.shoes, 0);
            handButton.Initialize(inventoryManager.Armor.hand, 0);

            for (int i = 0; i < playerInventoryButtons.Length; i++)
            {
                playerInventoryButtons[i].Initialize(inventoryManager.ItemsHeld[i], inventoryManager.NumberOfItems[i]);
            }

            SetupBackupIcon(inventoryManager);
            SetupItemIcon(helmetButton, helmetIcon);
            SetupItemIcon(bodyButton, bodyIcon);
            SetupItemIcon(shoesButton, shoesIcon);
            SetupItemIcon(handButton, handIcon);

            SetupHealthSlider(playerStats);
            SetupTirednessSlider(playerStats);
            SetupHungrySldier(playerStats);
        }

        private void UpdateBackpackSlots()
        {
            if (backpackButton.Item?.itemSO != null)
            {
                if (CursorUI.Instance.ItemCursor is BackpackInstance backpackInstance)
                {
                    for (int i = 0; i < backpackInstance.content.Length; i++)
                    {
                        backpackInstance.content[i] = backbackInventoryButtons[i].Item;
                        backpackInstance.amount[i] = backbackInventoryButtons[i].Amount;
                    }
                }
                for (int i = 0; i < backbackInventoryButtons.Length; i++)
                {
                    backbackInventoryButtons[i].Initialize(((BackpackInstance)backpackButton.Item).content[i], ((BackpackInstance)backpackButton.Item).amount[i]);
                }
                backpackBlockedText.SetActive(false);
                backpackContent.SetActive(true);
                backpackIcon.gameObject.SetActive(false);
            }
            else
            {
                backpackBlockedText.SetActive(true);
                backpackContent.SetActive(false);
                backpackIcon.gameObject.SetActive(true);
                if (CursorUI.Instance.ItemCursor?.itemSO != null && CursorUI.Instance.ItemCursor is BackpackInstance backpackInstance)
                {
                    for (int i = 0; i < backpackInstance.content.Length; i++)
                    {
                        backpackInstance.content[i] = backbackInventoryButtons[i].Item;
                        backpackInstance.amount[i] = backbackInventoryButtons[i].Amount;
                    }
                }
            }
        }

        private void SetupBackupIcon(InventoryManager inventoryManager)
        {
            if (backpackButton.Item?.itemSO != null)
            {
                for (int i = 0; i < backbackInventoryButtons.Length; i++)
                {
                    backbackInventoryButtons[i].Initialize(inventoryManager.Armor.backpack.content[i], inventoryManager.Armor.backpack.amount[i]);
                }
                backpackBlockedText.SetActive(false);
                backpackContent.SetActive(true);
                backpackIcon.gameObject.SetActive(false);
            }
            else
            {
                backpackBlockedText.SetActive(true);
                backpackContent.SetActive(false);
                backpackIcon.gameObject.SetActive(true);
            }
        }

        private void SetupItemIcon(InventoryButtonUI inventoryButton, Image icon)
        {
            if (inventoryButton.Item?.itemSO != null)
                icon.gameObject.SetActive(false);
            else
                icon.gameObject.SetActive(true);
        }

        private void SetupHealthSlider(PlayerStats playerStats)
        {
            healthSlider.value = playerStats.Health;
            healthStatusText.SetText($"HEALTH: {(int)playerStats.Health} / {playerStats.MaxHealth} ");
        }

        private void SetupTirednessSlider(PlayerStats playerStats)
        {
            tirednessSlider.value = playerStats.Tiredness;
            tirednessStatusText.SetText($"TIREDNESS: {(int)playerStats.Tiredness} / {playerStats.MaxTiredness} ");
        }

        private void SetupHungrySldier(PlayerStats playerStats)
        {
            hungrySlider.value = playerStats.Hunger;
            hungryStatusText.SetText($"HUNGER: {(int)playerStats.Hunger} / {playerStats.MaxHunger} ");
        }
    }
}
