using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{
    public class CookingUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private InventoryButtonUI foodToCookButton;
        [SerializeField] private InventoryButtonUI fuelButton;
        [SerializeField] private InventoryButtonUI cookedFoodButton;

        [SerializeField] private Image foodIcon;
        [SerializeField] private Image fireIcon;

        [SerializeField] private Slider cookingStatusSlider;

        [SerializeField] private InventoryButtonUI[] playerInventoryButtons;

        [SerializeField] private UIManager UIManager;

        private CookingItem chooseFurnace;

        public virtual void Start()
        {
            CookingItem.OnCooked += CookingItem_OnCooked;

            foodToCookButton.AddAdditionalListner(() =>
            {
                chooseFurnace.SetFoodToCooke(foodToCookButton.Item, foodToCookButton.Amount);
                SetupFoodIcon();

            });

            fuelButton.AddAdditionalListner(() =>
            {
                chooseFurnace.SetFuel(fuelButton.Item, fuelButton.Amount);
                SetupFuelIcon();
            });

            cookedFoodButton.AddAdditionalListner(() =>
            {
                chooseFurnace.SetCookedFood(cookedFoodButton.Item, cookedFoodButton.Amount);
            });

            closeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                if (CursorUI.Instance.ItemCursor == null)
                {
                    InventoryManager.Instance.UpdateInventory(playerInventoryButtons, true);
                    CursorUI.Instance.Hide();
                    Hide();
                    UIManager.SetGameState(UIOpenState.None);
                    chooseFurnace = null;
                }
            });

            Hide();
        }

        private void CookingItem_OnCooked(object sender, EventArgs e)
        {
            if (chooseFurnace)
            {
                foodToCookButton.Initialize(chooseFurnace.FoodToCooked, chooseFurnace.FoodToCookedAmount);
                fuelButton.Initialize(chooseFurnace.Fuel, chooseFurnace.FuelAmount);
                cookedFoodButton.Initialize(chooseFurnace.CookedFood, chooseFurnace.CookedFoodAmount);

                SetupFoodIcon();
                SetupFuelIcon();
            }
        }
        private void OnDestroy() => CookingItem.OnCooked -= CookingItem_OnCooked;

        public void CookingItem_OnInteract(object sender, System.EventArgs e)
        {
            chooseFurnace = sender as CookingItem;

            foodToCookButton.Initialize(chooseFurnace.FoodToCooked, chooseFurnace.FoodToCookedAmount);
            fuelButton.Initialize(chooseFurnace.Fuel, chooseFurnace.FuelAmount);
            cookedFoodButton.Initialize(chooseFurnace.CookedFood, chooseFurnace.CookedFoodAmount);
            UIManager.SetGameState(UIOpenState.ItemUI);
            SetupFoodIcon();
            SetupFuelIcon();

            Show();
            for (int i = 0; i < playerInventoryButtons.Length; i++)
            {
                playerInventoryButtons[i].Initialize(InventoryManager.Instance.ItemsHeld[i], InventoryManager.Instance.NumberOfItems[i]);
            }
        }

        private void Update()
        {
            if (chooseFurnace.FoodToCooked != null)
                cookingStatusSlider.value = chooseFurnace.FoodToCooked.strength;
            else
                cookingStatusSlider.value = 0;
        }

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

        private void SetupFoodIcon()
        {
            if (foodToCookButton.Item != null)
            {
                foodIcon.gameObject.SetActive(false);
                cookingStatusSlider.maxValue = foodToCookButton.Item.itemSO.cookingTime;
            }
            else
                foodIcon.gameObject.SetActive(true);
        }

        private void SetupFuelIcon()
        {
            if (fuelButton.Item != null)
                fireIcon.gameObject.SetActive(false);
            else
                fireIcon.gameObject.SetActive(true);
        }
    }
}
