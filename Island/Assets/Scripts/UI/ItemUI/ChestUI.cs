using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{
    public class ChestUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private InventoryButtonUI[] chestContentButtons;
        [SerializeField] private InventoryButtonUI[] playerInventoryButtons;

        [SerializeField] private UIManager UIManager;

        private Chest chooseChest;

        private void Start()
        {
            Chest.OnInteract += Chest_OnInteract;

            closeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                if (CursorUI.Instance.ItemCursor?.itemSO == null)
                {
                    for (int i = 0; i < chestContentButtons.Length; i++)
                    {
                        chooseChest.SetItemInChest(chestContentButtons[i].Item, i);
                        chooseChest.SetItemAmountInChest(chestContentButtons[i].Amount, i);
                    }
                    InventoryManager.Instance.UpdateInventory(playerInventoryButtons, true);
                    CursorUI.Instance.Hide();
                    chooseChest.CloseChestAnim();
                    Hide();
                    UIManager.SetGameState(UIOpenState.None);
                }
            });

            Hide();
        }

        private void Chest_OnInteract(object sender, System.EventArgs e)
        {
            chooseChest = sender as Chest;
            UIManager.SetGameState(UIOpenState.ItemUI);
            Show();

            for (int i = 0; i < playerInventoryButtons.Length; i++)
                playerInventoryButtons[i].Initialize(InventoryManager.Instance.ItemsHeld[i], InventoryManager.Instance.NumberOfItems[i]);

            for (int i = 0; i < chestContentButtons.Length; i++)
                chestContentButtons[i].Initialize(chooseChest.ItemInChest[i], chooseChest.ItemsAmountInChest[i]);
        }
        private void OnDestroy() => Chest.OnInteract -= Chest_OnInteract;

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
