using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island.Item
{

    [CreateAssetMenu(fileName = "InteractItemSO", menuName = "ItemSO/InteractItemSO", order = 0)]
    public class InteractItemSO : ItemSO
    {
        [SerializeField] private InteractItem interactItem;

        [SerializeField] private float positionOffset = 2.2f;

        public override bool Use(PlayerController player)
        {
            Vector2 position = player.transform.position + new Vector3(player.LastDirectionVector.x * positionOffset, player.LastDirectionVector.y * positionOffset);
            return SpawnManager.SpawnInteractObject(interactItem, position);
        }

    }
}

