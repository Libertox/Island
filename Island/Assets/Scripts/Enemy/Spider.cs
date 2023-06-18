using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Island.Enemy
{
    public class Spider : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemySO enemySO;
        [SerializeField] private int amountOfSpawnedItem;
        private NavMeshAgent navMeshAgent;
        private PlayerController target;
        private float time;
        private float health;

        public Vector2 LastDirection { get; private set; }
        public Vector2 Direction => new Vector2(navMeshAgent.velocity.x, navMeshAgent.velocity.y);
        public bool IsWalking { get; private set; }
        public bool IsAttacking { get; private set; }

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;

            health = enemySO.maxHealth;
        }

        private void Update()
        {
            if (FindTarget())
            {
                navMeshAgent.SetDestination(target.transform.position);
                if (TargetAchieved())
                    Attack();    
                else
                {
                    if (navMeshAgent.velocity != Vector3.zero)
                            LastDirection = new Vector2(navMeshAgent.velocity.x, navMeshAgent.velocity.y).normalized;

                    IsWalking = true;
                    IsAttacking = false;
                }
            }
            else
                IsWalking = false;
        }

        private bool FindTarget()
        {
            Collider2D collider2D = Physics2D.OverlapCircle(transform.position, enemySO.radiusRange, enemySO.playerLayerMask);
            if (collider2D)
            {
                target = collider2D.GetComponent<PlayerController>();
                return true;
            }
            else
                target = null;

            return false;
        }

        public bool TargetAchieved()
        {
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        return true;
                }
            }

            return false;
        }

        private void Attack()
        {
            IsWalking = false;
            IsAttacking = true;
            time += Time.deltaTime;
            if (time > enemySO.attackCooldown)
            {
                AudioManager.Instance.PlaySoundEffect(enemySO.audioClip);
                time = 0;
                target.PlayerStats.GetDamage(enemySO.damage);
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0)
            {
                Instantiate(enemySO.deadEffect, transform.position, Quaternion.identity);
                SpawnLoot();
                Destroy(gameObject);
            }
        }

        private void SpawnLoot()
        {
            for (int i = 0; i < amountOfSpawnedItem; i++)
            {
                int firstItem = Random.Range(0, enemySO.spawnedItem.Count);
                Instantiate(enemySO.spawnedItem[firstItem].itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}