using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.UI
{
	public class MoveChooserUIInput : MonoBehaviour
	{
		[SerializeField, Required]
		private UIBattleMoveChooser battleMoveChooserUI;

		[SerializeField, Required]
		private InputReader inputReader;

		[ShowInInspector, ReadOnly]
		private Vector2Int _input;

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
			battleMoveChooserUI.CancelSelection();
		}

		private void SelectCurrent()
		{
			battleMoveChooserUI.SelectActiveSelection();
		}

		private void UpdateInput(Vector2 dir)
		{
			int tabInput = (int)dir.x.Sign0();
			int selectionInput = (int)dir.y.Sign0();

			if (tabInput != _input.x)
			{
				battleMoveChooserUI.IncrementTab(tabInput);
				_arrTimer = inputReader.DelayedAutoShiftSeconds;
			}

			if (selectionInput != _input.y)
			{
				battleMoveChooserUI.IncrementSelection(selectionInput);
				_arrTimer = inputReader.DelayedAutoShiftSeconds;
			}
			
			_input = new Vector2Int(tabInput, selectionInput);
		}

		private void Update()
		{
			if (_input == Vector2Int.zero) return;

			_arrTimer = Mathf.Max(_arrTimer - Time.deltaTime, 0);
			if (_arrTimer == 0)
			{
				battleMoveChooserUI.IncrementTab(_input.x);
				battleMoveChooserUI.IncrementSelection(_input.y);
				_arrTimer = inputReader.AutoRepeatRateSeconds;
			}
		}
	}
}