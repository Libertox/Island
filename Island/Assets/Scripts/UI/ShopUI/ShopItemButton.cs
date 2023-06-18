using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class ShopItemButton : MonoBehaviour
    {
        [SerializeField] private InventorySingleUI inventorySingle;
        public InventorySingleUI InventorySingleUI => inventorySingle;
        public Button ItemButton { get; private set; }
        public int ItemIndex { get; private set; }

        private void Awake() => ItemButton = GetComponent<Button>();

        public void SetItemIndex(int index) => ItemIndex = index;
    }
}
