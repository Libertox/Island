using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Island.Item;

namespace Island.UI
{
    public class InventorySingleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI numberOfItemText;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image itemStrengthBackground;
        [SerializeField] private Image itemStrengthBar;

        public void UpdateVisual(ItemInstance itemData, int amount)
        {
            if (itemData?.itemSO != null)
            {
                itemIcon.gameObject.SetActive(true);
                itemIcon.sprite = itemData.itemSO.itemIcon;

                if (itemData.itemSO.itemType.HasFlag(ItemType.Single) && !itemData.itemSO.itemType.HasFlag(ItemType.Backpack))
                {
                    itemStrengthBackground.gameObject.SetActive(true);
                    numberOfItemText.gameObject.SetActive(false);
                    itemStrengthBar.fillAmount = itemData.strength / itemData.itemSO.maxStrength;
                }
                else
                {
                    numberOfItemText.SetText($"{amount}");
                    itemStrengthBackground.gameObject.SetActive(false);
                    numberOfItemText.gameObject.SetActive(true);
                }
            }
            else
                DisabledElements();
        }

        private void DisabledElements()
        {
            itemStrengthBackground.gameObject.SetActive(false);
            numberOfItemText.gameObject.SetActive(false);
            itemIcon.gameObject.SetActive(false);
        }

    }
}
