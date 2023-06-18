using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class Trap : InteractItem
    {
        private const string IS_CATCH = "IsCatch";

        public float CatchTime { get; private set; }
        public bool IsCatch { get; private set; }

        [SerializeField] private Animator animator;
        [SerializeField] private float catchCooldown;
        [SerializeField] private ItemSO foodSO;

        private void Update()
        {
            if (!IsCatch)
            {
                CatchTime += Time.deltaTime;
                if (CatchTime >= catchCooldown)
                {
                    IsCatch = true;
                    CatchTime -= catchCooldown;
                    animator.SetBool(IS_CATCH, IsCatch);
                }
            }

        }

        public override void Interact(PlayerController player)
        {
            if (IsCatch)
            {
                if (InventoryManager.Instance.AddItem(new ItemInstance(foodSO, 0)))
                {
                    IsCatch = false;
                    animator.SetBool(IS_CATCH, IsCatch);
                }
            }
        }

        public override void DestroySelf()
        {
            base.DestroySelf();
            if (IsCatch)
            {
                if (!InventoryManager.Instance.AddItem(new ItemInstance(foodSO, 0)))
                    Instantiate(foodSO.itemPrefab, transform.position, Quaternion.identity);
            }

        }

        public override ObjectSerializable CreateItemData() => new TrapSerializable(this);

        public void Initialize(TrapSerializable trapData)
        {
            transform.position = new Vector3(trapData.position.x, trapData.position.y, trapData.position.z);
            CatchTime = trapData.catchTime + TimeManager.Instance.GetExtraTime();
            IsCatch = trapData.isCatch;
            animator.SetBool(IS_CATCH, IsCatch);
        }

    }

    [System.Serializable]
    public class TrapSerializable : ObjectSerializable
    {
        public float catchTime;
        public bool isCatch;

        public TrapSerializable(Trap trap) : base(trap.transform.position)
        {
            catchTime = trap.CatchTime;
            isCatch = trap.IsCatch;
        }

        public override Object CreateObject(SaveManager saveSystem)
        {
            Trap trap = Object.Instantiate(saveSystem.PrefabList.trapPrefab);
            trap.Initialize(this);
            return trap;
        }
    }
}
