using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island
{
    [CreateAssetMenu(fileName = "PrefabLists", menuName = "PrefabListSO", order = 0)]
    public class PrefabListSO : ScriptableObject
    {
        public Anvil anvilPrefab;
        public Campfire campfirePrefab;
        public Furnace furnacePrefab;
        public Chest chestPrefab;
        public Tent tentPrefab;
        public Trap trapPrefab;
        public Plant plantPrefab;
        public BigTree treePrefab;
        public Boat boatPrefab;
        public Sign signPrefab;
        public Merchant merchantPrefab;
    }
}