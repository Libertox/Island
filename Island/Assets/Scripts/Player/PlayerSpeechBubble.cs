using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Island
{
    public class PlayerSpeechBubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speechBubbleText;
        [SerializeField] private float cooldownShowText;

        private CanvasGroup canvasGroup;
        private float displayTimer;
        private bool show;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        private void Update()
        {
            if (show)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime);
                if (canvasGroup.alpha == 1f)
                {
                    displayTimer += Time.deltaTime;
                    if (displayTimer > cooldownShowText)
                        show = false;
                }
            }
            else
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime);
                if (canvasGroup.alpha == 0f)
                    Hide();
            }

        }

        public void SetMessage(string message)
        {
            show = true;
            Show();
            speechBubbleText.SetText(message);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

    }
}

