using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "HammerSO", menuName = "ItemSO/HammerSO", order = 0)]
    public class HammerSO : ItemSO
    {
        public override bool Use(PlayerController player)
        {
            InteractItem interactItem = player.CheckInteractableType<InteractItem>();
            if (interactItem)
            {
                interactItem.DestroySelf();
                return true;
            }
            return false;
        }

        public override bool CanUse(PlayerController player) => player.CheckInteractableType<InteractItem>() != null;

    }
}
