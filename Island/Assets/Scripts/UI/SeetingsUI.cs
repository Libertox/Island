using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class SeetingsUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button soundEffectsButton;
        [SerializeField] private Button musicButton;

        [SerializeField] private TextMeshProUGUI soundEffectVolumeText;
        [SerializeField] private TextMeshProUGUI musicVolumeText;

        private void Start()
        {
            closeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                Hide();
            });

            soundEffectsButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                AudioManager.Instance.IncreaseSoundEffectVolume();
                UpdateSoundVolumeText();
            });

            musicButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                AudioManager.Instance.IncreaseMusicVolume();
                UpdateMusicVolumeText();
            });

            Hide();

            GameInput.Instance.OnPauseMenuOpened += GameInput_OnPauseMenuOpened;
        }

        private void GameInput_OnPauseMenuOpened(object sender, System.EventArgs e) => Hide();
        
        private void OnDestroy() => GameInput.Instance.OnPauseMenuOpened -= GameInput_OnPauseMenuOpened;
        
       
        public void Show()
        {
            UpdateMusicVolumeText();
            UpdateSoundVolumeText();

            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        private void UpdateSoundVolumeText()
        {
            int volume = (int)(AudioManager.Instance.SoundEffectVolume * 10);
            soundEffectVolumeText.SetText($"Sound Effects: {volume}");
        }

        private void UpdateMusicVolumeText()
        {
            int volume = (int)(AudioManager.Instance.MusicVolume * 10);
            musicVolumeText.SetText($"Music: {volume}");
        }
    }
}
