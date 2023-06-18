using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "SeedSO", menuName = "ItemSO/SeedSO", order = 0)]
    public class SeedSO : ItemSO
    {
        public Sprite stalk;
        public InteractObject.Tree smallPlant;
        public BigTree finalPlant;

        public float growingCooldown;

        public override bool Use(PlayerController player)
        {
            Plant plant = player.CheckInteractableType<Plant>();
            if (plant)
            {
                if (!plant.Seed)
                {
                    plant.SetSeed(this);
                    return true;
                }
            }
            return false;
        }
    }
}

