using System;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class TargetChooserUIInput : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleTargetChooserUI targetChooserUI;

		[SerializeField, Required]
		private InputReader inputReader;

		[ShowInInspector, ReadOnly]
		private int _input = 0;

		private float _arrTimer = 0f;

		private void OnEnable()
		{
			inputReader.MoveUIEvent += UpdateInput;
			inputReader.ConfirmUIEvent += SelectCurrent;
			inputReader.CancelUIEvent += CancelSelection;
		}

		private void OnDisable()
		{
			inputReader.MoveUIEvent -= UpdateInput;
			inputReader.ConfirmUIEvent -= SelectCurrent;
			inputReader.CancelUIEvent -= CancelSelection;
		}

		private void CancelSelection()
		{
			targetChooserUI.CancelSelection();
		}

		private void SelectCurrent()
		{
			targetChooserUI.SelectCurrentTarget();
		}

		private void UpdateInput(Vector2 dir)
		{
			int sign = (int)dir.x.Sign0();

			if (sign != _input)
			{
				targetChooserUI.IncrementSelectionIndex(sign);
				_arrTimer = inputReader.DelayedAutoShiftSeconds;
			}
			
			_input = sign;
		}

		private void Update()
		{
			if (_input == 0) return;

			_arrTimer = Mathf.Max(_arrTimer - Time.deltaTime, 0);
			if (_arrTimer == 0)
			{
				targetChooserUI.IncrementSelectionIndex(_input);
				_arrTimer = inputReader.AutoRepeatRateSeconds;
			}
		}
	}
}