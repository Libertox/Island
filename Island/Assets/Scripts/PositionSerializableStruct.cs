using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    [System.Serializable]
    public struct PositionSerializableStruct
    {
        public float x;
        public float y;
        public float z;

        public PositionSerializableStruct(Vector3 position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }
    }
}
