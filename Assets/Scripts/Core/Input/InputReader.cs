using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace SaturnRPG
{
    [CreateAssetMenu(menuName = "Core/Input/Input Reader")]
    public class InputReader : ScriptableObject, MainInput.IBattleActions, MainInput.IUIActions
    {
        private MainInput _mainInput;

        [field: SerializeField]
        public float AutoRepeatRateSeconds { get; private set; } = 0.05f;

        [field: SerializeField]
        public float DelayedAutoShiftSeconds { get; private set; } = 0.2f;


        private void OnEnable()
        {
            if (_mainInput == null)
            {
                _mainInput = new MainInput();
                
                _mainInput.Battle.SetCallbacks(this);
                _mainInput.UI.SetCallbacks(this);
                
            }
            _stateStack.Clear();
            PushState(InputState.UI);
        }

        public event Action<Vector2> MoveUIEvent;
        public event Action ConfirmUIEvent;
        public event Action CancelUIEvent;

        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> AimDirectionEvent;
        public event Action<Vector2> MouseDeltaEvent;
        public event Action<bool> ShiftEvent;
        public event Action<bool> PrimaryEvent;
        public event Action<bool> SecondaryEvent;

        [ShowInInspector, ReadOnly]
        private readonly Stack<InputState> _stateStack = new();

        public enum InputState
        {
            Battle,
            UI
        }

        public void PushState(InputState newState)
        {
            Debug.Log($"new state: {newState}");
            _stateStack.Push(newState);
            SetState(newState);
        }

        public void PopState()
        {
            _stateStack.Pop();
            SetState(_stateStack.Peek());
        }

        private void SetState(InputState newState)
        {
            switch (newState)
            {
                case InputState.Battle:
                    SetBattle();
                    break;
                case InputState.UI:
                    SetUI();
                    break;
            }
        }

        [Button, DisableInEditorMode]
        private void SetBattle()
        {
            _mainInput.Battle.Enable();
            _mainInput.UI.Disable();
        }

        [Button, DisableInEditorMode]
        private void SetUI()
        {
            _mainInput.UI.Enable();
            _mainInput.Battle.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAimDirection(InputAction.CallbackContext context)
        {
            AimDirectionEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMouseDelta(InputAction.CallbackContext context)
        {
            MouseDeltaEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnShift(InputAction.CallbackContext context)
        {
            ShiftEvent?.Invoke(context.started || context.performed);
        }

        public void OnPrimary(InputAction.CallbackContext context)
        {
            PrimaryEvent?.Invoke(context.started || context.performed);
        }

        public void OnSecondary(InputAction.CallbackContext context)
        {
            SecondaryEvent?.Invoke(context.started || context.performed);
        }

        public void OnMoveUI(InputAction.CallbackContext context)
        {
            MoveUIEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.performed)
                ConfirmUIEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
                CancelUIEvent?.Invoke();
        }
    }
}
