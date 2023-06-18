using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class RebindButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bindText;
        [SerializeField] private GameInput.Binding binding;

        [SerializeField] private GameObject rebindMessage;
        [SerializeField] private TextMeshProUGUI rebindText;

        private Button rebindButton;

        private void Awake() => rebindButton = GetComponent<Button>();

        private void Start()
        {
            UpdateBindText();
            rebindButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                ShowRebindMessage();
                rebindText.SetText("PRESS A KEY TO REBIND");
                GameInput.Instance.RebindBinding(binding, () =>
                {
                    UpdateBindText();
                    HideRebindMessage();
                }, (message => rebindText.SetText(message)));

            });
        }

        private void UpdateBindText() => bindText.SetText(GameInput.Instance.GetBindingText(binding));

        private void ShowRebindMessage() => rebindMessage.SetActive(true);

        private void HideRebindMessage() => rebindMessage.SetActive(false);
    }
}