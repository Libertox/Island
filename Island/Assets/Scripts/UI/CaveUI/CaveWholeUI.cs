using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{

    public class CaveWholeUI : ChoicePanelUI
    {
        private CaveWhole caveWhole;

        public override void Start()
        {
            CaveWhole.OnInteract += CaveWhole_OnInteract;
            acceptButton.onClick.AddListener(() => UIManager.TransitionBetweenLevels(() => SceneLoader.LoadScene(caveWhole.TargetScence)));
            base.Start();
        }

        private void CaveWhole_OnInteract(object sender, System.EventArgs e)
        {
            caveWhole = sender as CaveWhole;
            Show();
            UIManager.SetGameState(UIOpenState.ItemUI);
        }

        private void OnDestroy() => CaveWhole.OnInteract -= CaveWhole_OnInteract;

    }
}
