using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "SwordSO", menuName = "ItemSO/SwordSO", order = 0)]
    public class SwordSO : ItemSO
    {
        public int damage;
        public override bool Use(PlayerController player)
        {

            if (player.EnemyTarget != null)
            {
                player.EnemyTarget.TakeDamage(damage);
                return true;
            }
            return false;
        }

        public override bool CanUse(PlayerController player) => player.EnemyTarget != null;
    }
}
