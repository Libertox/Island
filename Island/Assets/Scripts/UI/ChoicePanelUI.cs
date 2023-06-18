
using UnityEngine;
using UnityEngine.UI;

namespace Island.UI
{
    public class ChoicePanelUI : MonoBehaviour
    {
        [SerializeField] protected Button acceptButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] protected UIManager UIManager;

        public virtual void Start()
        {
            acceptButton.onClick.AddListener(() =>
            {
                UIManager.SetGameState(UIOpenState.None);
                AudioManager.Instance.PlayButtonSoundEffect();
                Hide();
            });

            cancelButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                Hide();
                UIManager.SetGameState(UIOpenState.None);
            });

            Hide();

        }

        public void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
