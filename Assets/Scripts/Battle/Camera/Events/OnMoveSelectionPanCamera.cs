using System;
using SaturnRPG.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.Events
{
	public class OnMoveSelectionPanCamera : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleCamera battleCamera;
		
		[SerializeField, Required]
		private UIBattleMoveChooser battleMoveChooser;

		private void OnEnable()
		{
			battleMoveChooser.OnStartSelection += SetTargetUnit;
			battleMoveChooser.OnEndSelection += ResetCameraControl;
		}

		private void OnDisable()
		{
			battleMoveChooser.OnStartSelection -= SetTargetUnit;
			battleMoveChooser.OnEndSelection -= ResetCameraControl;
		}

		private void SetTargetUnit(BattleUnit unit)
		{
			battleCamera.SetTarget(unit);
		}
		
		private void ResetCameraControl()
		{
			battleCamera.ClearTarget();
		}
	}
}