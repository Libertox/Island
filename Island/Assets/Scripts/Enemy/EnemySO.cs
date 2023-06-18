using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;


namespace Island.Enemy
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "EnemySO")]
    public class EnemySO : ScriptableObject
    {
        public int maxHealth;
        public int damage;
        public float radiusRange;
        public LayerMask playerLayerMask;
        public float attackCooldown;
        public GameObject deadEffect;
        public List<ItemSO> spawnedItem;
        public AudioClip audioClip;
    }
}
