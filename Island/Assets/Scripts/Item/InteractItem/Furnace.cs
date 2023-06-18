using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class Furnace : CookingItem
    {
        public static event EventHandler OnInteract;

        [SerializeField] private GameObject fire;

        public override void Update()
        {
            base.Update();
            UpdateFireSprite();
        }

        public override void Interact(PlayerController player) => OnInteract?.Invoke(this, EventArgs.Empty);

        private void UpdateFireSprite()
        {
            if (CanCook())
            {
                if (!fire.activeSelf)
                    fire.SetActive(true);
            }
            else
            {
                if (fire.activeSelf)
                    fire.SetActive(false);
            }
        }

        public override ObjectSerializable CreateItemData() => new FurnaceSerializable(this);

    }
    [System.Serializable]
    public class FurnaceSerializable : ObjectSerializable
    {
        public ItemInstance foodToCooked;
        public int foodToCookedAmount;
        public ItemInstance fuel;
        public int fuelAmount;
        public ItemInstance cookedFood;
        public int cookedFoodAmount;

        public FurnaceSerializable(Furnace cookingItem) : base(cookingItem.transform.position)
        {
            foodToCooked = cookingItem.FoodToCooked;
            foodToCookedAmount = cookingItem.FoodToCookedAmount;
            fuel = cookingItem.Fuel;
            fuelAmount = cookingItem.FuelAmount;
            cookedFood = cookingItem.CookedFood;
            cookedFoodAmount = cookingItem.CookedFoodAmount;
        }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Furnace furnace = UnityEngine.Object.Instantiate(saveSystem.PrefabList.furnacePrefab);

            if (foodToCooked != null)
                foodToCooked.strength += TimeManager.Instance.GetExtraTime();

            furnace.transform.position = new Vector3(position.x, position.y, position.z);
            furnace.SetCookedFood(saveSystem.CreateLoadedItem(cookedFood), cookedFoodAmount);
            furnace.SetFoodToCooke(saveSystem.CreateLoadedItem(foodToCooked), foodToCookedAmount);
            furnace.SetFuel(saveSystem.CreateLoadedItem(fuel), fuelAmount);
            return furnace;
        }
    }
}
