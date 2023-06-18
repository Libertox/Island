using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Island.InteractObject;

namespace Island.UI
{
    public class SignUI : ChoicePanelUI
    {
        [SerializeField] private TextMeshProUGUI inscriptionsText;

        private Sign sign;

        public override void Start()
        {
            Sign.OnInteract += Sign_OnInteract;
            acceptButton.onClick.AddListener(() =>  sign.BuildObject());
            base.Start();
        }


        private void Sign_OnInteract(object sender, System.EventArgs e)
        {
            sign = sender as Sign;
            inscriptionsText.SetText(sign.Inscription);
            UIManager.SetGameState(UIOpenState.ItemUI);
            Show();
        }

        private void OnDestroy() => Sign.OnInteract -= Sign_OnInteract;

    }
}
