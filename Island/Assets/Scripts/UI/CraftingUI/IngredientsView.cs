using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Island.Item;

namespace Island.UI
{
    public class IngredientsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private Image[] IngredientsImage;
        [SerializeField] private TextMeshProUGUI[] IngredientsText;

        [SerializeField] private Image finalProductImage;

        [SerializeField] private TextMeshProUGUI itemProperties;

        private void Start() => Hide();

        public void Setup(ItemSO itemSO)
        {
            Show();

            itemNameText.SetText(itemSO.itemName);
            itemProperties.SetText(itemSO.properties);

            for (int i = 0; i < IngredientsText.Length; i++)
            {
                IngredientsText[i].gameObject.SetActive(false);
                IngredientsImage[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < itemSO.needIngredientsToCrafting.Count; i++)
            {
                IngredientsImage[i].sprite = itemSO.needIngredientsToCrafting[i].itemIcon;
                IngredientsImage[i].gameObject.SetActive(true);

                if (InventoryManager.Instance.FindItemIndex(itemSO.needIngredientsToCrafting[i], out int index))
                    IngredientsText[i].SetText($"{InventoryManager.Instance.NumberOfItems[index]} / {itemSO.needAmountToCrafting[i]}");
                else
                    IngredientsText[i].SetText($"0 / {itemSO.needAmountToCrafting[i]}");

                IngredientsText[i].gameObject.SetActive(true);
            }

            finalProductImage.sprite = itemSO.itemIcon;

        }


        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}