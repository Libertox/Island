using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class CookingItem : InteractItem
    {
        public static event EventHandler OnCooked;

        public ItemInstance FoodToCooked { get; private set; }
        public int FoodToCookedAmount { get; private set; }
        public ItemInstance Fuel { get; private set; }
        public int FuelAmount { get; private set; }
        public ItemInstance CookedFood { get; private set; }
        public int CookedFoodAmount { get; private set; }

        public virtual void Update()
        {
            if (CanCook())
            {
                FoodToCooked.strength += Time.deltaTime * Fuel.itemSO.burnSpeed;
                if (FoodToCooked.strength > FoodToCooked.itemSO.cookingTime)
                {
                    if (CookedFood?.itemSO != FoodToCooked.itemSO.cookFood)
                    {
                        CookedFood = new ItemInstance(FoodToCooked.itemSO.cookFood, 0);
                        CookedFoodAmount++;
                    }
                    else
                        CookedFoodAmount++;

                    FoodToCooked.strength = 0;

                    FoodToCookedAmount--;
                    if (FoodToCookedAmount == 0)
                        FoodToCooked = null;

                    FuelAmount--;
                    if (FuelAmount == 0)
                        Fuel = null;

                    OnCooked?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        protected bool CanCook() => FoodToCooked != null && Fuel != null && (CookedFood == null || CookedFood?.itemSO == FoodToCooked?.itemSO.cookFood);


        public override void DestroySelf()
        {
            base.DestroySelf();
            AddItemsAfterDestroy(FoodToCooked, FoodToCookedAmount);
            AddItemsAfterDestroy(Fuel, FuelAmount);
            AddItemsAfterDestroy(CookedFood, CookedFoodAmount);
        }
        private void AddItemsAfterDestroy(ItemInstance itemInstnace, int amount)
        {
            if (itemInstnace != null && !InventoryManager.Instance.AddItem(itemInstnace, amount))
            {
                for (int i = 0; i < amount; i++)
                    SpawnBaseItem(itemInstnace);
            }
        }

        public void SetFoodToCooke(ItemInstance itemInstance, int amount)
        {
            FoodToCooked = itemInstance;
            FoodToCookedAmount = amount;
        }

        public void SetFuel(ItemInstance itemInstance, int amount)
        {
            Fuel = itemInstance;
            FuelAmount = amount;
        }

        public void SetCookedFood(ItemInstance itemInstance, int amount)
        {
            CookedFood = itemInstance;
            CookedFoodAmount = amount;
        }
    }
}