using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "FoodSO", menuName = "ItemSO/FoodSO", order = 0)]
    public class FoodSO : ItemSO
    {
        [SerializeField] private int eatValue;

        public override bool Use(PlayerController player)
        {
            player.PlayerStats.ChangeHunger(eatValue);
            return true;
        }
    }
}
