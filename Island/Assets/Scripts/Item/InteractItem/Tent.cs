using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{
    public class Tent : InteractItem
    {
        public static event EventHandler OnInteract;

        private const string I_AM_NOT_TIRED = "I am not tired";

        public override void Interact(PlayerController player)
        {
            if (player.PlayerStats.Tiredness < 60)
                OnInteract?.Invoke(this, EventArgs.Empty);
            else
                player.SpeechBubble.SetMessage(I_AM_NOT_TIRED);

            player.SetTargetSecne(0);
        }

        public override ObjectSerializable CreateItemData() => new TentSerializable(this);

    }

    [System.Serializable]
    public class TentSerializable : ObjectSerializable
    {
        public TentSerializable(Tent tent) : base(tent.transform.position) { }

        public override UnityEngine.Object CreateObject(SaveManager saveSystem)
        {
            Tent tent = UnityEngine.Object.Instantiate(saveSystem.PrefabList.tentPrefab);
            tent.transform.position = new Vector3(position.x, position.y, position.z);
            return tent;
        }

    }
}