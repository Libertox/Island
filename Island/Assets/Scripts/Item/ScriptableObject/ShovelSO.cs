using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "ShovelSO", menuName = "ItemSO/ShovelSO", order = 0)]
    public class ShovelSO : ItemSO
    {
        public Plant spaceForPlantPrefab;

        [SerializeField] private float positionOffset;
        public override bool Use(PlayerController player)
        {
            Vector2 position = player.transform.position + new Vector3(player.LastDirectionVector.x * positionOffset, player.LastDirectionVector.y * positionOffset);
            return SpawnManager.SpawnInteractObject(spaceForPlantPrefab, position);
        }

    }
}
