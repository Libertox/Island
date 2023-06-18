using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Island.UI
{
    public class CraftingMessage : MonoBehaviour
    {
        [SerializeField] private float awakeCooldown;
        [SerializeField] private TextMeshProUGUI messageText;

        private float displayTimer;
        private bool show;
        private CanvasGroup canvasGroup;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        private void Update()
        {
            if (show)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime);
                if (canvasGroup.alpha == 1f)
                {
                    displayTimer += Time.deltaTime;
                    if (displayTimer > awakeCooldown)
                        show = false;
                }
            }
            else
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime);
                if (canvasGroup.alpha == 0f)
                {
                    Hide();
                    displayTimer = 0;
                }

            }
        }

        public void Activate(string message)
        {
            messageText.SetText(message);
            Show();
            show = true;
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}