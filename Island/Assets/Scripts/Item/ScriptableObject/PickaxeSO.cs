using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island.Item
{

    [CreateAssetMenu(fileName = "PickaxeSO", menuName = "ItemSO/PickaxeSO", order = 0)]
    public class PickaxeSO : ItemSO
    {
        public override bool Use(PlayerController player)
        {
            Stone stone = player.CheckInteractableType<Stone>();
            if (stone)
            {
                stone.Dig();
                return true;
            }
            return false;
        }

        public override bool CanUse(PlayerController player) => player.CheckInteractableType<Stone>() != null;

    }
}
