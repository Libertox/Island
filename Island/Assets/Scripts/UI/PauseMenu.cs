using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private SeetingsUI seetingsUI;

        [SerializeField] private UIManager UIManager;

        private bool isShow;

        private void Start()
        {
            GameInput.Instance.OnPauseMenuOpened += GameInput_OnPauseMenuOpened;

            resumeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                Show();
            });

            saveButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                SaveGame();
            });

            settingsButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                OpenSeetingsWindow();
            });

            exitButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                BackToMainMenu();
            });

            Hide();
        }

        private void GameInput_OnPauseMenuOpened(object sender, System.EventArgs e)
        {
            if (!UIManager.IsDeathState())
                Show();
        }

        private void Show()
        {
            isShow = !isShow;
            gameObject.SetActive(isShow);

            UIManager.PauseGame();

            if (!SceneLoader.IsHomeScence())
                saveButton.gameObject.SetActive(false);
            else
                saveButton.gameObject.SetActive(true);
        }

        private void Hide() => gameObject.SetActive(false);

        private void SaveGame() => SaveManager.Instance.SaveGame();

        private void OpenSeetingsWindow() => seetingsUI.Show();

        private void BackToMainMenu()
        {
            Time.timeScale = 1f;
            SceneLoader.LoadScene(Scene.MainMenu);
            Destroy(PlayerController.Instance.gameObject);
            Destroy(UIManager.Instance.gameObject);
            Destroy(InventoryManager.Instance.gameObject);
            Destroy(GameInput.Instance.gameObject);
            Destroy(TimeManager.Instance.gameObject);
            Destroy(SaveManager.Instance.gameObject);
        }
    }
}
