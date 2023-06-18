using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class Merchant : MonoBehaviour, Interactable, IWriteable
    {
        public static event EventHandler OnInteract;

        [SerializeField] private float cooldownToChnageItem;
        [SerializeField] private ItemsListSO itemsListSO;
        [SerializeField] private ItemSO coinSO;

        public ItemSO CoinSO => coinSO;
        public ItemSO[] ItemToSell { get; private set; } = new ItemSO[3];
        public int[] ItemToSellAmount { get; private set; } = new int[3];
        public float Time { get; private set; }
        public bool UpdateShopInventory { get; set; }

        private void Start()
        {
            if (!PlayerPrefs.HasKey(SceneLoader.GetCurrentSceneName()))
            {
                RandomGenerateItem();
                Time = cooldownToChnageItem;
            }

        }
        private void Update()
        {
            Time -= UnityEngine.Time.deltaTime;
            if (Time <= 0)
            {
                Time += cooldownToChnageItem;
                RandomGenerateItem();
                UpdateShopInventory = true;
            }
        }

        private void RandomGenerateItem()
        {
            int maxAmountRange = 25;
            for (int i = 0; i < ItemToSell.Length; i++)
            {
                ItemToSell[i] = itemsListSO.itemsList[UnityEngine.Random.Range(0, itemsListSO.itemsList.Count)];
                ItemToSellAmount[i] = UnityEngine.Random.Range(1, maxAmountRange);
            }
        }

        public void Initialize(MerchantSerializable data)
        {

            Time = data.time - TimeManager.Instance.GetExtraTime();
            for (int i = 0; i < ItemToSell.Length; i++)
                ItemToSell[i] = SaveManager.Instance.FindItemSO(data.itemName[i]);

            ItemToSellAmount = data.itemAmount;
        }


        public void RemoveSellItem(int index)
        {
            if (ItemToSellAmount[index] >= 1)
                ItemToSellAmount[index] -= 1;

            if (ItemToSellAmount[index] == 0)
                ItemToSell[index] = null;
        }

        public void Interact(PlayerController player) => OnInteract?.Invoke(this, EventArgs.Empty);

        public ObjectSerializable CreateItemData() => new MerchantSerializable(this);

    }

    [System.Serializable]
    public class MerchantSerializable : ObjectSerializable
    {
        public float time;
        public int[] itemAmount;
        public string[] itemName = new string[3];

        public MerchantSerializable(Merchant merchant) : base(merchant.transform.position)
        {
            time = merchant.Time;
            itemAmount = merchant.ItemToSellAmount;
            for (int i = 0; i < itemName.Length; i++)
                itemName[i] = merchant.ItemToSell[i]?.name;
        }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Merchant merchant = UnityEngine.Object.Instantiate(saveSystem.PrefabList.merchantPrefab);
            merchant.transform.position = new Vector3(position.x, position.y, position.z);
            merchant.Initialize(this);
            return merchant;
        }
    }
}