using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.Item;

namespace Island.UI
{
    public class CursorUI : MonoBehaviour
    {
        public static CursorUI Instance { get; private set; }

        public ItemInstance ItemCursor { get; private set; }
        public int ItemAmount { get; private set; }
        private Image itemIcon;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            itemIcon = GetComponent<Image>();
            Hide();
        }

        private void Update() => transform.position = Input.mousePosition;

        public ItemInstance SwapItem(ItemInstance item)
        {
            ItemInstance temp = ItemCursor;
            ItemCursor = item;

            if (ItemCursor?.itemSO == null)
                Hide();
            else
            {
                transform.position = Input.mousePosition;
                itemIcon.sprite = ItemCursor.itemSO.itemIcon;
                Show();
            }

            return temp;
        }

        public int SwapAmountItem(int amount)
        {
            int tempAmount = ItemAmount;
            ItemAmount = amount;
            return tempAmount;
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

    }
}
