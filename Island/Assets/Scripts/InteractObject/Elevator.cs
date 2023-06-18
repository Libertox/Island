using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{
    public class Elevator : MonoBehaviour, Interactable
    {
        public static event EventHandler OnInteract;

        public void Interact(PlayerController player) => OnInteract?.Invoke(this, EventArgs.Empty);
    }
}