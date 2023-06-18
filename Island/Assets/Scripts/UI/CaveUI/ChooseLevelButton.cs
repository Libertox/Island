using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class ChooseLevelButton : MonoBehaviour
    {
        public Button chooseLevelButton { get; private set; }
        [SerializeField] private Scene sceneToLoad;

        private void Awake() => chooseLevelButton = GetComponent<Button>();
        private void Start() => chooseLevelButton.onClick.AddListener(() =>
        {
            UIManager.Instance.TransitionBetweenLevels(() => SceneLoader.LoadScene(sceneToLoad));
            AudioManager.Instance.PlayButtonSoundEffect();
        });

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        public void Active()
        {
            if (PlayerPrefs.HasKey(sceneToLoad.ToString()))
            {
                if (PlayerPrefs.GetInt(sceneToLoad.ToString()) == 0)
                    Show();
                else
                    Hide();
            }
            else
                Hide();
        }
    }
}
