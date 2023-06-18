using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "AxeSO", menuName = "ItemSO/AxeSO", order = 0)]
    public class AxeSO : ItemSO
    {
        public override bool Use(PlayerController player)
        {
            InteractObject.Tree tree = player.CheckInteractableType<InteractObject.Tree>();
            if (tree)
            {
                tree.Cut();
                return true;
            }
            return false;
        }

        public override bool CanUse(PlayerController player) => player.CheckInteractableType<InteractObject.Tree>() != null;

    }
}
