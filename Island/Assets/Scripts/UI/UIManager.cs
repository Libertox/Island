using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.UI;

namespace Island
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private UIOpenState gameState = UIOpenState.None;
        private UIOpenState PreviousGameState = UIOpenState.None;

        [SerializeField] private FadeImage fadeImage;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public bool IsNoneState() => gameState == UIOpenState.None;

        public bool IsPauseState() => gameState == UIOpenState.PauseUI;

        public bool IsCraftingState() => gameState == UIOpenState.CraftingUI;

        public bool IsInventoryState() => gameState == UIOpenState.InventoryUI;

        public bool IsDeathState() => gameState == UIOpenState.DeathUI;

        public void SetGameState(UIOpenState state)
        {
            PreviousGameState = gameState;
            gameState = state;

        }

        public void TransitionBetweenLevels(Action otherAction = null)
        {
            fadeImage.Show(() =>
            {
                otherAction?.Invoke();
                SaveManager.Instance.SaveSceneObject();
                SetGameState(UIOpenState.None);
               
            });
        }

        public void PauseGame()
        {
            if (IsPauseState())
            {
                Time.timeScale = 1f;
                SetGameState(PreviousGameState);
            }
            else
            {
                SetGameState(UIOpenState.PauseUI);
                Time.timeScale = 0f;
            }
        }

    }
    public enum UIOpenState
    {
        None,
        PauseUI,
        CraftingUI,
        InventoryUI,
        ItemUI,
        DeathUI,
    }
}