using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island
{
    public interface IWriteable
    {
        public ObjectSerializable CreateItemData();
    }
}
