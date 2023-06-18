using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class DeathUI : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private UIManager UIManager;

        private void Start()
        {
            PlayerController.Instance.PlayerStats.OnDied += PlayerStats_OnDied;

            restartButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                UIManager.TransitionBetweenLevels(() => 
                {
                    PlayerController.Instance.PlayerStats.RestartStatistic();
                    SceneLoader.LoadScene(Scene.Home);
                });

                Time.timeScale = 1f;
                Hide();
            });

            Hide();
        }

        private void PlayerStats_OnDied(object sender, System.EventArgs e)
        {
            Show();
            Time.timeScale = 0f;
            UIManager.SetGameState(UIOpenState.DeathUI);
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
