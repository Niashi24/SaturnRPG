using System;
using LS.Utilities;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SaturnRPG.Battle.BattleAction
{
	[SelectionBase]
	public abstract class PlayerActionComponent : MonoBehaviour
	{
		[SerializeField, Required, Header("Action Component")]
		private InputReader inputReader;

		[SerializeField]
		private ValueReference<Camera> mainCamera;

		[SerializeField, Required]
		protected Transform playerTransform;

		[SerializeField, Min(4)]
		private int controllerAimNotches = 32;

		// [ShowInInspector, ReadOnly]
		public ActionInput MainInput { get; private set; } = new();

		private bool useMouse = false;

		protected virtual void Awake()
		{
			inputReader.PushState(InputReader.InputState.Battle);

			inputReader.MoveEvent += UpdateMove;
			inputReader.AimDirectionEvent += UpdateAimDirection;
			inputReader.MouseDeltaEvent += OnMouseDelta;
			inputReader.ShiftEvent += UpdateShift;
			inputReader.PrimaryEvent += UpdatePrimary;
			inputReader.SecondaryEvent += UpdateSecondary;
		}

		protected virtual void OnDestroy()
		{
			inputReader.PopState();
			inputReader.MoveEvent -= UpdateMove;
			inputReader.AimDirectionEvent -= UpdateAimDirection;
			inputReader.MouseDeltaEvent -= OnMouseDelta;
			inputReader.ShiftEvent -= UpdateShift;
			inputReader.PrimaryEvent -= UpdatePrimary;
			inputReader.SecondaryEvent -= UpdateSecondary;
		}

		protected virtual void Update()
		{
			if (useMouse)
				UpdateMouse();
		}

		private void UpdateMouse()
		{
			MainInput.AimDirection =
				((Vector2)playerTransform.position).DirectionTo(mainCamera.Value.GetMouseWorldPosition());
		}

		private void UpdateMove(Vector2 moveDirection)
			=> MainInput.MoveDirection = moveDirection;

		private void UpdateAimDirection(Vector2 aimDirection)
		{
			useMouse = false;
			MainInput.AimDirection = aimDirection
				.Angle()
				.RoundTo(2 * Mathf.PI / controllerAimNotches)
				.AngleToDirection();
		}

		private void OnMouseDelta(Vector2 _)
		{
			useMouse = true;
			UpdateMouse();
		}

		private void UpdateShift(bool shift)
		{
			if (!MainInput.Shift && shift)
				MainInput.OnShift?.Invoke();
			if (MainInput.Shift && !shift)
				MainInput.OffShift?.Invoke();
			MainInput.Shift = shift;
		}

		private void UpdatePrimary(bool primary)
		{
			if (!MainInput.Primary && primary)
				MainInput.OnPrimary?.Invoke();
			if (MainInput.Primary && !primary)
				MainInput.OffPrimary?.Invoke();
			MainInput.Primary = primary;
		}

		private void UpdateSecondary(bool secondary)
		{
			if (!MainInput.Secondary && secondary)
				MainInput.OnSecondary?.Invoke();
			if (MainInput.Secondary && !secondary)
				MainInput.OffSecondary?.Invoke();
			MainInput.Secondary = secondary;
		}
	}
}