using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.Item;

namespace Island.UI
{
    public class CraftingUI : MonoBehaviour
    {
        private const string INVENTORY_FULL = "Inventory is full";
        private const string ENOUGH_INGREDIENTS = "Not enough ingredients";

        [SerializeField] private CraftingSingleButton craftingSingleButtonTemplate;
        [SerializeField] private Transform buttonContener;

        [SerializeField] private ItemsListSO creatingItemsListSO;
        [SerializeField] private IngredientsView receptionView;
        [SerializeField] private CraftingMessage craftingMessage;

        [SerializeField] private Button createButton;

        [SerializeField] private UIManager UIManager;

        private ItemSO chooseItem;
        private bool isShow;

        private void Start()
        {
            GameInput.Instance.OnCraftingUIOpened += GameInput_OnCraftingUIOpened;

            foreach (ItemSO itemSO in creatingItemsListSO.itemsList)
            {
                CraftingSingleButton craftingSingleButton = Instantiate(craftingSingleButtonTemplate, buttonContener);
                craftingSingleButton.Initialize(itemSO, this);
            }
            createButton.onClick.AddListener(() =>
            {
                CraftItem();
                AudioManager.Instance.PlayButtonSoundEffect();
            });

            Hide();
        }

        private void GameInput_OnCraftingUIOpened(object sender, System.EventArgs e)
        {
            if (!UIManager.IsNoneState() && !UIManager.IsCraftingState())
                return;

            isShow = !isShow;
            if (isShow)
            {
                UIManager.SetGameState(UIOpenState.CraftingUI);
                if (chooseItem != null)
                    receptionView.Setup(chooseItem);
            }
            else
                UIManager.SetGameState(UIOpenState.None);
            gameObject.SetActive(isShow);
        }

        public void SelectItem(ItemSO itemSO)
        {
            chooseItem = itemSO;
            receptionView.Setup(chooseItem);
        }

        private void CraftItem()
        {
            bool isEnough = false;
            int[] index = new int[chooseItem.needIngredientsToCrafting.Count];

            for (int i = 0; i < chooseItem.needIngredientsToCrafting.Count; i++)
            {
                isEnough = InventoryManager.Instance.CheckEnoughItem(chooseItem.needIngredientsToCrafting[i], chooseItem.needAmountToCrafting[i], out index[i]);
                if (!isEnough) break;
            }


            if (isEnough)
            {
                if (!InventoryManager.Instance.AddItem(new ItemInstance(chooseItem, chooseItem.maxStrength)))
                    craftingMessage.Activate(INVENTORY_FULL);
                else
                {
                    for (int i = 0; i < index.Length; i++)
                        InventoryManager.Instance.RemoveItemOnIndex(index[i], chooseItem.needAmountToCrafting[i]);

                    receptionView.Setup(chooseItem);
                }
            }
            else
                craftingMessage.Activate(ENOUGH_INGREDIENTS);
        }

        private void Hide() => gameObject.SetActive(false);

    }
}
