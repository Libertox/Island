using System;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class FadeImage : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeTime;

        private float time;
        private readonly int fadeSpeed = 2;
        private Action actionAfterFade;
        private FadeState fadeState;

        private enum FadeState
        {
            ToFade,
            IsFade,
            FromFade
        }

        private void Update()
        {
            switch (fadeState)
            {

                case FadeState.ToFade:
                    fadeImage.fillAmount = Mathf.MoveTowards(fadeImage.fillAmount, 1, Time.deltaTime * fadeSpeed);
                    if (fadeImage.fillAmount == 1)
                        fadeState = FadeState.IsFade;
                    break;
                case FadeState.IsFade:
                    actionAfterFade();
                    fadeState = FadeState.FromFade;
                    break;
                case FadeState.FromFade:
                    time += Time.deltaTime;
                    if (time > fadeTime)
                    {
                        fadeImage.fillAmount = Mathf.MoveTowards(fadeImage.fillAmount, 0, Time.deltaTime * fadeSpeed);
                        if (fadeImage.fillAmount == 0)
                        {
                            if (SceneLoader.IsHomeScence())
                                SaveManager.Instance.SaveGame();
                            time = 0;
                            Hide();

                        }
                    }
                    break;
            }
        }
        public void Show(Action action)
        {
            gameObject.SetActive(true);
            fadeState = FadeState.ToFade;
            actionAfterFade = action;
        }

        private void Hide() => gameObject.SetActive(false);

    }
}
