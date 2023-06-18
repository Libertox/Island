using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{

    public class Boat : AreaExit, Interactable, IWriteable
    {
        public static event EventHandler OnInteract;

        public ObjectSerializable CreateItemData() => new BoatSerializable(this);

        public override void Interact(PlayerController player)
        {
            player.SetTargetSecne(AreaTransaciton);
            OnInteract?.Invoke(this, EventArgs.Empty);
        }

    }

    [System.Serializable]
    public class BoatSerializable : ObjectSerializable
    {
        public BoatSerializable(Boat boat) : base(boat.transform.position) { }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Boat boat = UnityEngine.Object.Instantiate(saveSystem.PrefabList.boatPrefab);
            boat.transform.position = new Vector3(position.x, position.y, position.z);
            return boat;
        }
    }
}