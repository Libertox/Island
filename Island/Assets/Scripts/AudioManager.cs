using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private const string PLAYER_PREFS_SOUND_EFFECT = "SoundEffect";
        private const string PLAYER_PREFS_MUSIC = "Music";

        private readonly float maxSoundEffectVolume = 1f;
        private readonly float maxMusicVolume = 1f;

        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource soundEffectAudioSource;
        [SerializeField] private SoundEffectsSO soundEffectList;

        public float SoundEffectVolume => soundEffectAudioSource.volume;
        public float MusicVolume => musicAudioSource.volume;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            if (PlayerPrefs.HasKey(PLAYER_PREFS_MUSIC))
                musicAudioSource.volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC);

            if (PlayerPrefs.HasKey(PLAYER_PREFS_SOUND_EFFECT))
                soundEffectAudioSource.volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT);
        }

        public void IncreaseSoundEffectVolume()
        {

            if (soundEffectAudioSource.volume == maxSoundEffectVolume)
                soundEffectAudioSource.volume = 0f;
            else
                soundEffectAudioSource.volume += 0.1f;

            PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT, SoundEffectVolume);
            PlayerPrefs.Save();
        }

        public void IncreaseMusicVolume()
        {
            if (musicAudioSource.volume == maxMusicVolume)
                musicAudioSource.volume = 0f;
            else
                musicAudioSource.volume += 0.1f;

            PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC, MusicVolume);
            PlayerPrefs.Save();
        }

        public void PlaySoundEffect(AudioClip audioClip)
        {
            if (audioClip != null && !soundEffectAudioSource.isPlaying)
                soundEffectAudioSource.PlayOneShot(audioClip);
        }

        public void PlayButtonSoundEffect()
        {
            soundEffectAudioSource.PlayOneShot(soundEffectList.buttonsSound[Random.Range(0, soundEffectList.buttonsSound.Length)]);
        }

    }
}
