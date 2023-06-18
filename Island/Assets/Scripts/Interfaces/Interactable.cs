using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public interface Interactable
    {
        public void Interact(PlayerController player);
    }
}