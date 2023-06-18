using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Island.InteractObject;

namespace Island.UI
{
    public class LadderUI : ChoicePanelUI
    {
        private Ladder ladder;

        public override void Start()
        {
            Ladder.OnInteract += CaveWhole_OnInteract;
            acceptButton.onClick.AddListener(() => UIManager.TransitionBetweenLevels(() => SceneLoader.LoadScene(ladder.TargetScence)));
            base.Start();
        }

        private void CaveWhole_OnInteract(object sender, System.EventArgs e)
        {
            ladder = sender as Ladder;
            Show();
            UIManager.SetGameState(UIOpenState.ItemUI);
        }

        private void OnDestroy() => Ladder.OnInteract -= CaveWhole_OnInteract;

    }
}

