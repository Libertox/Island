using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{
    public class BigTree : Tree, IWriteable
    {

        public void Initialize(TreeSerializable data)
        {
            transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
            health = data.health;
        }
        public ObjectSerializable CreateItemData() => new TreeSerializable(this);
    }
    [System.Serializable]
    public class TreeSerializable : ObjectSerializable
    {
        public int health;

        public TreeSerializable(BigTree tree) : base(tree.transform.position)
        {
            health = tree.Health;
        }

        public override Object CreateObject(SaveManager saveSystem)
        {
            BigTree tree = Object.Instantiate(saveSystem.PrefabList.treePrefab);
            tree.Initialize(this);
            return tree;
        }

    }
}
