
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private MapButton[] mapButtons;

        [SerializeField] private UIManager UIManager;

        private void Start()
        {
            Boat.OnInteract += Boat_OnInteract;
            for (int i = 0; i < mapButtons.Length; i++)
                mapButtons[i].LoadLocationButton.onClick.AddListener(() => Hide());

            closeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                Hide();
                UIManager.SetGameState(UIOpenState.None);
            });

            Hide();
        }

        private void Boat_OnInteract(object sender, System.EventArgs e)
        {
            Show();
            for (int i = 0; i < mapButtons.Length; i++)
                mapButtons[i].Active();
            UIManager.SetGameState(UIOpenState.ItemUI);
        }

        private void OnDestroy() => Boat.OnInteract -= Boat_OnInteract;

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);
    }
}
