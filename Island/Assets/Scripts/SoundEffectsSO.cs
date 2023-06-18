using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    [CreateAssetMenu(fileName = "SoundEffectsSO", menuName = "SoundEffectsSO")]
    public class SoundEffectsSO : ScriptableObject
    {
        public AudioClip[] buttonsSound;
    }
}
