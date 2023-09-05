using System;
using LS.Utilities;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	[SelectionBase]
	public abstract class PlayerActionComponent : MonoBehaviour
	{
		[SerializeField, Required]
		private InputReader inputReader;

		[SerializeField]
		private ValueReference<Camera> mainCamera;

		[SerializeField, Required]
		protected Transform playerTransform;

		[SerializeField, Min(4)]
		private int controllerAimNotches = 32;

		// [ShowInInspector, ReadOnly]
		protected ActionInput mainInput = new();

		private bool useMouse = false;

		protected virtual void Awake()
		{
			inputReader.SetBattle();

			inputReader.MoveEvent += UpdateMove;
			inputReader.AimDirectionEvent += UpdateAimDirection;
			inputReader.MouseDeltaEvent += OnMouseDelta;
			inputReader.ShiftEvent += UpdateShift;
			inputReader.PrimaryEvent += UpdatePrimary;
			inputReader.SecondaryEvent += UpdateSecondary;
		}

		protected virtual void OnDestroy()
		{
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
			{
				mainInput.AimDirection =
					((Vector2)playerTransform.position).DirectionTo(mainCamera.Value.GetMouseWorldPosition());
			}
		}

		private void UpdateMouse()
		{
			mainInput.AimDirection =
				((Vector2)playerTransform.position).DirectionTo(mainCamera.Value.GetMouseWorldPosition());
		}

		private void UpdateMove(Vector2 moveDirection)
			=> mainInput.MoveDirection = moveDirection;

		private void UpdateAimDirection(Vector2 aimDirection)
		{
			useMouse = false;
			mainInput.AimDirection = aimDirection
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
			if (!mainInput.Shift && shift)
				mainInput.OnShift?.Invoke();
			if (mainInput.Shift && !shift)
				mainInput.OffShift?.Invoke();
			mainInput.Shift = shift;
		}

		private void UpdatePrimary(bool primary)
		{
			if (!mainInput.Primary && primary)
				mainInput.OnPrimary?.Invoke();
			if (mainInput.Primary && !primary)
				mainInput.OffPrimary?.Invoke();
			mainInput.Primary = primary;
		}

		private void UpdateSecondary(bool secondary)
		{
			if (!mainInput.Secondary && secondary)
				mainInput.OnSecondary?.Invoke();
			if (mainInput.Secondary && !secondary)
				mainInput.OffSecondary?.Invoke();
			mainInput.Secondary = secondary;
		}
	}
}