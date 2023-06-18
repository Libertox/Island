using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{

    public class Anvil : InteractItem
    {
        public static event EventHandler OnInteract;

        public ItemInstance RepairedItem { get; set; }

        public override void Interact(PlayerController player) => OnInteract?.Invoke(this, EventArgs.Empty);

        public override void DestroySelf()
        {
            base.DestroySelf();
            if (RepairedItem != null)
            {
                if (!InventoryManager.Instance.AddItem(RepairedItem))
                    SpawnBaseItem(RepairedItem);
            }
        }

        public override ObjectSerializable CreateItemData() => new AnvilSerializable(this);
    }

    [System.Serializable]
    public class AnvilSerializable : ObjectSerializable
    {
        public ItemInstance repairedItem;

        public AnvilSerializable(Anvil anvil) : base(anvil.transform.position)
        {
            repairedItem = anvil.RepairedItem;
        }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Anvil anvil = UnityEngine.Object.Instantiate(saveSystem.PrefabList.anvilPrefab);
            anvil.transform.position = new Vector3(position.x, position.y, position.z);
            anvil.RepairedItem = saveSystem.CreateLoadedItem(repairedItem);
            return anvil;
        }

    }
}

