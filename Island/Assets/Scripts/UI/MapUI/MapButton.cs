using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class MapButton : MonoBehaviour
    { 
        [SerializeField] private Scene targetLocation;
        [SerializeField] private GameObject playerIndicator;
        public Button LoadLocationButton { get; private set; }

        private void Awake() => LoadLocationButton = GetComponent<Button>();

        private void Start() => LoadLocationButton.onClick.AddListener(() =>
        {
            UIManager.Instance.TransitionBetweenLevels(() => SceneLoader.LoadScene(targetLocation));
            AudioManager.Instance.PlayButtonSoundEffect();
        });

        public void Active()
        {
            if (SceneLoader.GetCurrentSceneName() == targetLocation.ToString())
                playerIndicator.SetActive(true);
            else
                playerIndicator.SetActive(false);
        }
    }
}
