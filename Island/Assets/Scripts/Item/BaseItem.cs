using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    public class BaseItem : MonoBehaviour, Interactable
    {
        [SerializeField] private ItemSO itemSO;
        public ItemSO ItemSO => itemSO;

        [SerializeField] private float strength;
        public float ItemStrength { get => strength; set => strength = value; }

        public virtual void Interact(PlayerController player)
        {
            if (InventoryManager.Instance.AddItem(new ItemInstance(itemSO, strength)))
                DestroySelf();
        }

        public void DestroySelf() => Destroy(gameObject);
    }
}
