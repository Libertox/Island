using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "KeySO", menuName = "ItemSO/KeySO", order = 0)]
    public class KeySO : ItemSO
    {

        public override bool Use(PlayerController player)
        {
            Chest chest = player.CheckInteractableType<Chest>();
            if (chest)
            {
                if (chest.IsLock())
                {
                    chest.SetLock(false);
                    return true;
                }
            }
            return false;
        }
    }
}
