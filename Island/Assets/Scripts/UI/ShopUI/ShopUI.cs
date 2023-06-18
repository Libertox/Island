using Island.InteractObject;
using Island.Item;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private ShopItemButton[] sellItemButtons;
        [SerializeField] private ShopItemButton[] buyItemButtons;

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI priceText;

        [SerializeField] private Button sellButton;
        [SerializeField] private Button buyButton;

        [SerializeField] private UIManager UIManager;

        private int chooseItemIndex;
        private Merchant merchant;
        private InventoryManager inventoryManager;

        private void Start()
        {
            Merchant.OnInteract += Merchant_OnInteract;

            closeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                UIManager.SetGameState(UIOpenState.None);
                chooseItemIndex = -1;
                Hide();
            });

            SetupSellButtons();
            SetupBuyButtons();

            sellButton.onClick.AddListener(() =>
            {
                SellItem();
                AudioManager.Instance.PlayButtonSoundEffect();
            });
            buyButton.onClick.AddListener(() =>
            {
                BuyItem();
                AudioManager.Instance.PlayButtonSoundEffect();
            });

            Hide();
        }


        private void SetupBuyButtons()
        {
            foreach (ShopItemButton buyItemButton in buyItemButtons)
            {
                buyItemButton.ItemButton.onClick.AddListener(() =>
                {
                    chooseItemIndex = buyItemButton.ItemIndex;
                    if (merchant.ItemToSell[chooseItemIndex] != null)
                    {
                        priceText.gameObject.SetActive(true);
                        priceText.SetText(merchant.ItemToSell[chooseItemIndex].buyValue.ToString());
                        sellButton.gameObject.SetActive(false);
                        buyButton.gameObject.SetActive(true);
                    }
                    else
                        HideButtons();
                });
            }
        }

        private void SetupSellButtons()
        {
            foreach (ShopItemButton sellItemButton in sellItemButtons)
            {
                sellItemButton.ItemButton.onClick.AddListener(() =>
                {
                    chooseItemIndex = sellItemButton.ItemIndex;
                    if (inventoryManager.ItemsHeld[chooseItemIndex] != null)
                    {
                        priceText.gameObject.SetActive(true);
                        priceText.SetText(inventoryManager.ItemsHeld[chooseItemIndex].itemSO.sellValue.ToString());
                        sellButton.gameObject.SetActive(true);
                        buyButton.gameObject.SetActive(false);
                    }
                    else
                        HideButtons();
                });
            }
        }

        private void OnDestroy()
        {
            Merchant.OnInteract -= Merchant_OnInteract;
        }

        private void Update()
        {
            UpdateTimer();
            if (merchant.UpdateShopInventory)
            {
                UpdateBuyButtons();
                HideButtons();
                chooseItemIndex = -1;
                merchant.UpdateShopInventory = false;
            }
        }

        private void Merchant_OnInteract(object sender, System.EventArgs e)
        {
            merchant = sender as Merchant;
            inventoryManager = InventoryManager.Instance;
            UIManager.Instance.SetGameState(UIOpenState.ItemUI);
            UpdateSellButtons();
            UpdateBuyButtons();
            HideButtons();
            Show();
        }

        private void UpdateTimer()
        {
            int second = (int)(merchant.Time % 60);
            int minute = (int)merchant.Time / 60;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(minute);
            stringBuilder.Append(":");
            stringBuilder.Append(second);
            timeText.SetText(stringBuilder.ToString());
        }

        private void HideButtons()
        {
            sellButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            priceText.gameObject.SetActive(false);
        }

        private void UpdateSellButtons()
        {
            for (int i = 0; i < sellItemButtons.Length; i++)
            {
                sellItemButtons[i].SetItemIndex(i);
                sellItemButtons[i].InventorySingleUI.UpdateVisual(inventoryManager.ItemsHeld[i], inventoryManager.NumberOfItems[i]);
            }
        }

        private void UpdateBuyButtons()
        {
            for (int i = 0; i < buyItemButtons.Length; i++)
            {
                buyItemButtons[i].SetItemIndex(i);
                if (merchant.ItemToSell[i] != null)
                    buyItemButtons[i].InventorySingleUI.UpdateVisual(new ItemInstance(merchant.ItemToSell[i], merchant.ItemToSell[i].maxStrength), merchant.ItemToSellAmount[i]);
                else
                    buyItemButtons[i].InventorySingleUI.UpdateVisual(null, 0);
            }
        }

        private void BuyItem()
        {
            if (!inventoryManager.CheckEnoughItem(merchant.CoinSO, merchant.ItemToSell[chooseItemIndex].buyValue, out int coinIndex))
                return;

            ItemInstance itemInstance;
            if (merchant.ItemToSell[chooseItemIndex].itemType.HasFlag(ItemType.Backpack))
                itemInstance = new BackpackInstance(merchant.ItemToSell[chooseItemIndex], merchant.ItemToSell[chooseItemIndex].maxStrength, new ItemInstance[28], new int[28]);
            else
                itemInstance = new ItemInstance(merchant.ItemToSell[chooseItemIndex], merchant.ItemToSell[chooseItemIndex].maxStrength);

            if (inventoryManager.AddItem(itemInstance))
            {
                inventoryManager.RemoveItemOnIndex(coinIndex, merchant.ItemToSell[chooseItemIndex].buyValue);
                merchant.RemoveSellItem(chooseItemIndex);

                UpdateSellButtons();

                if (merchant.ItemToSell[chooseItemIndex] != null)
                    buyItemButtons[chooseItemIndex].InventorySingleUI.UpdateVisual(new ItemInstance(merchant.ItemToSell[chooseItemIndex], merchant.ItemToSell[chooseItemIndex].maxStrength), merchant.ItemToSellAmount[chooseItemIndex]);
                else
                {
                    buyItemButtons[chooseItemIndex].InventorySingleUI.UpdateVisual(null, 0);
                    HideButtons();
                }
            }
        }

        private void SellItem()
        {
            if (inventoryManager.AddItem(new ItemInstance(merchant.CoinSO, merchant.CoinSO.maxStrength), inventoryManager.ItemsHeld[chooseItemIndex].itemSO.sellValue))
            {
                inventoryManager.RemoveItemOnIndex(chooseItemIndex, 1);
                UpdateSellButtons();

                if (inventoryManager.ItemsHeld[chooseItemIndex] == null)
                    HideButtons();
            }
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
