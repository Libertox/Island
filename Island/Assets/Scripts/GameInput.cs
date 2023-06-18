using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Island
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput Instance { get; private set; }

        private const string PLAYER_PREFS_BINDING = "InputBindings";

        public event EventHandler OnInteracted;
        public event EventHandler<OnItemSelectedEventArgs> OnItemSelected;
        public event EventHandler OnItemDroped;
        public event EventHandler<OnItemUsedEventArgs> OnItemUsed;
        public class OnItemSelectedEventArgs : EventArgs { public int selectedItem; }

        public class OnItemUsedEventArgs : EventArgs { public bool isUsed; }

        public event EventHandler OnPlayerUIOpened;
        public event EventHandler OnCraftingUIOpened;
        public event EventHandler OnPauseMenuOpened;

        public enum Binding
        {
            MoveUp,
            MoveDown,
            MoveRight,
            MoveLeft,
            Interact,
            UseItem,
            DropItem,
            SelectFristItem,
            SelectSecondItem,
            SelectThirdItem,
            SelectFourthItem,
            SelectFifthItem,
            SelectSixthItem,
            SelectSeventhItem,
            Inventory,
            Crafting,
        }

        private PlayerInput playerInput;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            playerInput = new PlayerInput();


            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING))
                playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));

            playerInput.Player.Enable();

            playerInput.Player.Interact.performed += Interact_performed;
            playerInput.Player.SelecetItem.performed += SelecetItem_performed;
            playerInput.Player.DropItem.performed += DropItem_performed;

            playerInput.Player.UseItem.performed += UseItem_performed;
            playerInput.Player.UseItem.canceled += UseItem_performed;

            playerInput.Player.PlayerUI.performed += PlayerUI_performed;
            playerInput.Player.CraftingUI.performed += CraftingUI_performed;
            playerInput.Player.PauseMenu.performed += PauseMenu_performed;

            

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            playerInput.Player.Interact.performed -= Interact_performed;
            playerInput.Player.SelecetItem.performed -= SelecetItem_performed;
            playerInput.Player.DropItem.performed -= DropItem_performed;

            playerInput.Player.UseItem.performed -= UseItem_performed;
            playerInput.Player.UseItem.canceled -= UseItem_performed;

            playerInput.Player.PlayerUI.performed -= PlayerUI_performed;
            playerInput.Player.CraftingUI.performed -= CraftingUI_performed;
            playerInput.Player.PauseMenu.performed -= PauseMenu_performed;

            playerInput.Dispose();
        }


        private void PauseMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnPauseMenuOpened?.Invoke(this, EventArgs.Empty);

        private void CraftingUI_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnCraftingUIOpened?.Invoke(this, EventArgs.Empty);

        private void PlayerUI_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnPlayerUIOpened?.Invoke(this, EventArgs.Empty);

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnInteracted?.Invoke(this, EventArgs.Empty);

        private void DropItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnItemDroped?.Invoke(this, EventArgs.Empty);

        private void UseItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnItemUsed?.Invoke(this, new OnItemUsedEventArgs
            {
                isUsed = obj.performed
            });
        }

        private void SelecetItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            int selectedItemIndex = 0;
            for(int i =0; i<playerInput.Player.SelecetItem.bindings.Count; i++)
            {
                if (playerInput.Player.SelecetItem.bindings[i].ToDisplayString() == obj.action.activeControl.displayName)
                {
                    selectedItemIndex = i + 1;
                    break;
                }          
            }
            OnItemSelected?.Invoke(this, new OnItemSelectedEventArgs
            {
                selectedItem = selectedItemIndex

            });

        }

        public Vector2 GetMovmentVectorNormalized()
        {
            if (UIManager.Instance.IsNoneState())
            {
                Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
                inputVector = inputVector.normalized;

                return inputVector;
            }
            return Vector2.zero;
        }

        public void RebindBinding(Binding binding, Action afterBindAction, Action<string> setBindText)
        {
            playerInput.Player.Disable();

            GetBindingInputAction(binding, out InputAction inputAction, out int bindingIndex);

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(callback =>
                {
                    callback.Dispose();
                    playerInput.Player.Enable();
                    afterBindAction();
                })
                .OnComplete(callback =>
                {
                    playerInput.Player.Enable();

                    if (CheckDuplicateBindings(inputAction, bindingIndex))
                    {
                        inputAction.RemoveBindingOverride(bindingIndex);
                        callback.Dispose();
                        setBindText("KEY IS USED");
                        RebindBinding(binding, afterBindAction, setBindText);
                        return;
                    }

                    callback.Dispose();
                    afterBindAction();

                    PlayerPrefs.SetString(PLAYER_PREFS_BINDING, playerInput.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();

                })
               .Start();    
        }

        private bool CheckDuplicateBindings(InputAction inputAction, int bindingIndex)
        {
            InputBinding newInputBinding = inputAction.bindings[bindingIndex];
            foreach (InputBinding inputBinding in inputAction.actionMap.bindings)
            {
                if (inputBinding.id == newInputBinding.id)
                    continue;

                if (inputBinding.effectivePath == newInputBinding.effectivePath)
                    return true;
            }

            return false;
        }

        public string GetBindingText(Binding binding)
        {
            switch (binding)
            {
                default:
                case Binding.MoveUp:
                    return playerInput.Player.Move.bindings[1].ToDisplayString();
                case Binding.MoveDown:
                    return playerInput.Player.Move.bindings[2].ToDisplayString();
                case Binding.MoveLeft:
                    return playerInput.Player.Move.bindings[3].ToDisplayString();
                case Binding.MoveRight:
                    return playerInput.Player.Move.bindings[4].ToDisplayString();
                case Binding.Interact:
                    return playerInput.Player.Interact.bindings[0].ToDisplayString();
                case Binding.UseItem:
                    return playerInput.Player.UseItem.bindings[0].ToDisplayString();
                case Binding.DropItem:
                    return playerInput.Player.DropItem.bindings[0].ToDisplayString();
                case Binding.SelectFristItem:
                    return playerInput.Player.SelecetItem.bindings[0].ToDisplayString();
                case Binding.SelectSecondItem:
                    return playerInput.Player.SelecetItem.bindings[1].ToDisplayString();
                case Binding.SelectThirdItem:
                    return playerInput.Player.SelecetItem.bindings[2].ToDisplayString();
                case Binding.SelectFourthItem:
                    return playerInput.Player.SelecetItem.bindings[3].ToDisplayString();
                case Binding.SelectFifthItem:
                    return playerInput.Player.SelecetItem.bindings[4].ToDisplayString();
                case Binding.SelectSixthItem:
                    return playerInput.Player.SelecetItem.bindings[5].ToDisplayString();
                case Binding.SelectSeventhItem:
                    return playerInput.Player.SelecetItem.bindings[6].ToDisplayString();
                case Binding.Inventory:
                    return playerInput.Player.PlayerUI.bindings[0].ToDisplayString();
                case Binding.Crafting:
                    return playerInput.Player.CraftingUI.bindings[0].ToDisplayString();
            }
        }

        private void GetBindingInputAction(Binding binding, out InputAction inputAction, out int bindingIndex)
        {
            switch (binding)
            {
                default:
                case Binding.MoveUp:
                    inputAction = playerInput.Player.Move;
                    bindingIndex = 1;
                    break;
                case Binding.MoveDown:
                    inputAction = playerInput.Player.Move;
                    bindingIndex = 2;
                    break;
                case Binding.MoveLeft:
                    inputAction = playerInput.Player.Move;
                    bindingIndex = 3;
                    break;
                case Binding.MoveRight:
                    inputAction = playerInput.Player.Move;
                    bindingIndex = 4;
                    break;
                case Binding.Interact:
                    inputAction = playerInput.Player.Interact;
                    bindingIndex = 0;
                    break;
                case Binding.UseItem:
                    inputAction = playerInput.Player.UseItem;
                    bindingIndex = 0;
                    break;
                case Binding.DropItem:
                    inputAction = playerInput.Player.DropItem;
                    bindingIndex = 0;
                    break;
                case Binding.SelectFristItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 0;
                    break;
                case Binding.SelectSecondItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 1;
                    break;
                case Binding.SelectThirdItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 2;
                    break;
                case Binding.SelectFourthItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 3;
                    break;
                case Binding.SelectFifthItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 4;
                    break;
                case Binding.SelectSixthItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 5;
                    break;
                case Binding.SelectSeventhItem:
                    inputAction = playerInput.Player.SelecetItem;
                    bindingIndex = 6;
                    break;
                case Binding.Inventory:
                    inputAction = playerInput.Player.PlayerUI;
                    bindingIndex = 0;
                    break;
                case Binding.Crafting:
                    inputAction = playerInput.Player.CraftingUI;
                    bindingIndex = 0;
                    break;
            }
        }
    }
}
