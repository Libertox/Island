using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Island.Item;

namespace Island.UI
{
    public class CraftingSingleButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI itemNameText;

        public void Initialize(ItemSO itemSO, CraftingUI craftingUI)
        {
            itemNameText.SetText(itemSO.itemName);
            button.onClick.AddListener(() =>
            {
                craftingUI.SelectItem(itemSO);
                AudioManager.Instance.PlayButtonSoundEffect();
            });
        }

    }
}
