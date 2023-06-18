using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private SeetingsUI settingsUI;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(Scene.Home.ToString()))
            {
                continueButton.gameObject.SetActive(false);
                SetButtonNavigation();
                newGameButton.Select();
            }
            else
                continueButton.Select();

            continueButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                SceneLoader.LoadScene(Scene.LoadingScene);
            });

            newGameButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                RestartGameProgress();
                SceneLoader.LoadScene(Scene.LoadingScene);
            });

            settingsButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                settingsUI.Show();
            });

            exitButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
    #if !UNITY_WEBGL
                Application.Quit();
    #endif
            });

        }

        private void SetButtonNavigation()
        {
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnUp = exitButton;
            navigation.selectOnDown = settingsButton;
            newGameButton.navigation = navigation;

            navigation.selectOnDown = newGameButton;
            navigation.selectOnUp = settingsButton;
            exitButton.navigation = navigation;
        }

        private void RestartGameProgress()
        {
            foreach (Scene scene in Enum.GetValues(typeof(Scene)))
                PlayerPrefs.DeleteKey(scene.ToString());
        }

    }
}