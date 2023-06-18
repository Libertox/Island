using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class InteractItem : MonoBehaviour, Interactable, IWriteable
    {
        [SerializeField] protected ItemSO itemToAdd;

        public virtual void Interact(PlayerController player) { }

        public virtual void DestroySelf()
        {
            if (!InventoryManager.Instance.AddItem(new ItemInstance(itemToAdd, 0)))
                Instantiate(itemToAdd.itemPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        public void SpawnBaseItem(ItemInstance itemInstance)
        {
            BaseItem baseItem = Instantiate(itemInstance.itemSO.itemPrefab, transform.position, Quaternion.identity);
            baseItem.ItemStrength = itemInstance.strength;
        }

        public virtual ObjectSerializable CreateItemData() => new ObjectSerializable(transform.position);

    }

    [System.Serializable]
    public class ObjectSerializable
    {
        public PositionSerializableStruct position;

        public ObjectSerializable(Vector3 pos)
        {
            position = new PositionSerializableStruct(pos);
        }

        public virtual Object CreateObject(SaveManager saveSystem) => null;

    }
}