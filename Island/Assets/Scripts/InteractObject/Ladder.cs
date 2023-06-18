using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{
    public class Ladder : AreaExit, Interactable
    {
        public static event EventHandler OnInteract;

        public override void Interact(PlayerController player)
        {
            player.SetTargetSecne(AreaTransaciton);
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
    }
}
