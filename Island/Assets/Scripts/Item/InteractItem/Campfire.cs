using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class Campfire : CookingItem
    {
        public static event EventHandler OnInteract;

        public override void Interact(PlayerController player) => OnInteract?.Invoke(this, EventArgs.Empty);

        public override ObjectSerializable CreateItemData() => new CampfireSerializable(this);
    }
    [System.Serializable]
    public class CampfireSerializable : ObjectSerializable
    {
        public ItemInstance foodToCooked;
        public int foodToCookedAmount;
        public ItemInstance fuel;
        public int fuelAmount;
        public ItemInstance cookedFood;
        public int cookedFoodAmount;

        public CampfireSerializable(Campfire cookingItem) : base(cookingItem.transform.position)
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
            Campfire campfire = UnityEngine.Object.Instantiate(saveSystem.PrefabList.campfirePrefab);

            if (foodToCooked != null)
                foodToCooked.strength += TimeManager.Instance.GetExtraTime();

            campfire.transform.position = new Vector3(position.x, position.y, position.z);
            campfire.SetCookedFood(saveSystem.CreateLoadedItem(cookedFood), cookedFoodAmount);
            campfire.SetFoodToCooke(saveSystem.CreateLoadedItem(foodToCooked), foodToCookedAmount);
            campfire.SetFuel(saveSystem.CreateLoadedItem(fuel), fuelAmount);
            return campfire;
        }
    }
}
