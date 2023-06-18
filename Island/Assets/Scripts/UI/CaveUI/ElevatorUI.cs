using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{
    public class ElevatorUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private List<ChooseLevelButton> chooseLevelButtonList;

        [SerializeField] private UIManager UIManager;

        private void Start()
        {
            Elevator.OnInteract += Elevator_OnInteract;

            closeButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayButtonSoundEffect();
                Hide();
                UIManager.SetGameState(UIOpenState.None);
            });

            foreach (ChooseLevelButton chooseLevelButton in chooseLevelButtonList)
            {
                chooseLevelButton.chooseLevelButton.onClick.AddListener(() =>
                {
                    AudioManager.Instance.PlayButtonSoundEffect();
                    Hide();
                    UIManager.SetGameState(UIOpenState.None);
                });
            }

            Hide();
        }

        private void Elevator_OnInteract(object sender, System.EventArgs e)
        {
            Show();
            UIManager.Instance.SetGameState(UIOpenState.ItemUI);
            foreach (ChooseLevelButton button in chooseLevelButtonList)
                button.Active();
        }

        private void OnDestroy() => Elevator.OnInteract -= Elevator_OnInteract;

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
