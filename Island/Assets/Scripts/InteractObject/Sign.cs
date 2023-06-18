using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{

    public class Sign : MonoBehaviour, Interactable, IWriteable
    {
        public static event EventHandler OnInteract;

        [SerializeField] private GameObject buildObjectPrefab;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private ItemSO needItem;
        [SerializeField] private int needAmount;
        [SerializeField] private string inscription;
        public string Inscription => inscription;
        public Transform SpawnPosition => spawnPosition;

        public void Interact(PlayerController player) => OnInteract?.Invoke(this, EventArgs.Empty);

        public ObjectSerializable CreateItemData() => new SignSerializable(this);

        public void Initialize(SignSerializable data)
        {
            transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
            spawnPosition.position = new Vector3(data.spawnPosition.x, data.spawnPosition.y, data.spawnPosition.z);
        }

        public void BuildObject()
        {
            if (InventoryManager.Instance.CheckEnoughItem(needItem, needAmount, out int index))
            {
                InventoryManager.Instance.RemoveItemOnIndex(index, needAmount);
                UIManager.Instance.TransitionBetweenLevels( () =>
                {
                    gameObject.SetActive(false);
                    Instantiate(buildObjectPrefab, spawnPosition.position, Quaternion.identity);
                });
            }
        }
    }

    [System.Serializable]
    public class SignSerializable : ObjectSerializable
    {
        public PositionSerializableStruct spawnPosition;

        public SignSerializable(Sign sign) : base(sign.transform.position)
        {
            spawnPosition = new PositionSerializableStruct(sign.SpawnPosition.position);
        }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Sign sign = UnityEngine.Object.Instantiate(saveSystem.PrefabList.signPrefab);
            sign.Initialize(this);
            return sign;
        }
    }
}