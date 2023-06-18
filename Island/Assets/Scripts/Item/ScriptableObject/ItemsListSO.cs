using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Item
{
    [CreateAssetMenu(fileName = "ItemsListSO", menuName = "ItemsListSO", order = 0)]
    public class ItemsListSO : ScriptableObject
    {
        public List<ItemSO> itemsList;
    }
}