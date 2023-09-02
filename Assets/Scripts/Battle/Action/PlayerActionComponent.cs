using System;
using LS.Utilities;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public abstract class PlayerActionComponent : MonoBehaviour
	{
		[SerializeField, Required]
		private InputReader inputReader;

		[SerializeField]
		private ValueReference<Camera> mainCamera;

		protected ActionInput mainInput = new();

		private void OnEnable()
		{
			inputReader.MoveEvent += UpdateMove;
			inputReader.AimDirectionEvent += UpdateAimDirection;
			inputReader.MouseDeltaEvent += UpdateAimDirectionMouse;
			inputReader.ShiftEvent += UpdateShift;
			inputReader.PrimaryEvent += UpdatePrimary;
			inputReader.SecondaryEvent += UpdateSecondary;
		}

		private void OnDisable()
		{
			inputReader.MoveEvent -= UpdateMove;
			inputReader.AimDirectionEvent -= UpdateAimDirection;
			inputReader.MouseDeltaEvent -= UpdateAimDirectionMouse;
			inputReader.ShiftEvent -= UpdateShift;
			inputReader.PrimaryEvent -= UpdatePrimary;
			inputReader.SecondaryEvent -= UpdateSecondary;
		}

		private void UpdateMove(Vector2 moveDirection)
			=> mainInput.MoveDirection = moveDirection;

		private void UpdateAimDirection(Vector2 aimDirection)
			=> mainInput.AimDirection = aimDirection;

		private void UpdateAimDirectionMouse(Vector2 delta)
		{
			var mousePos = mainCamera.Value.GetMouseWorldPosition();
			mainInput.AimDirection = (mousePos - (Vector2)transform.position).normalized;
		}

		private void UpdateShift(bool shift)
			=> mainInput.Shift = shift;

		private void UpdatePrimary(bool primary)
			=> mainInput.Primary = primary;

		private void UpdateSecondary(bool secondary)
			=> mainInput.Secondary = secondary;
	}
}