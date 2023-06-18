using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{
    public class Resource : MonoBehaviour, Interactable
    {
        [SerializeField] private int amountOfSpawnedItem;
        [SerializeField] private ItemSO[] spawnedItem;
        [SerializeField] private GameObject effect;
        [SerializeField] protected int health;

        [SerializeField] private ItemSO basicSpawnItem;

        public GameObject Effect => effect;
        public int Health => health;

        public void Interact(PlayerController player) { }

        public void SpawnIngredients()
        {
            Instantiate(basicSpawnItem.itemPrefab, transform.position, Quaternion.identity);
            for (int i = 0; i < amountOfSpawnedItem; i++)
            {
                int firstItem = Random.Range(0, spawnedItem.Length);
                Instantiate(spawnedItem[firstItem].itemPrefab, transform.position, Quaternion.identity);
            }
        }

    }
}
