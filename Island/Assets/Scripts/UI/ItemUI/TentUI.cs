
using Island.InteractObject;

namespace Island.UI
{
    public class TentUI : ChoicePanelUI
    {
        public override void Start()
        {
            Tent.OnInteract += Tent_OnInteract;

            acceptButton.onClick.AddListener(() =>
            {
                UIManager.TransitionBetweenLevels();
                PlayerController.Instance.PlayerStats.Sleep();
            });
            base.Start();
        }

        private void Tent_OnInteract(object sender, System.EventArgs e)
        {
            Show();
            UIManager.SetGameState(UIOpenState.ItemUI);
        }

        private void OnDestroy() => Tent.OnInteract -= Tent_OnInteract;
    }
}