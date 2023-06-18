using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{

    public class DestroyParticleSystem : MonoBehaviour
    {
        [SerializeField] private float effectLifeTime;

        private void Update() => Destroy(gameObject, effectLifeTime);

    }
}