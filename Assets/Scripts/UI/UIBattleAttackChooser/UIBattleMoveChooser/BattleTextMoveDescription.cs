using System;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class BattleTextMoveDescription : MonoBehaviour
	{
		[SerializeField, Required]
		private UIBattleMoveChooser battleMoveChooser;

		[SerializeField, Required]
		private BattleText battleText;

		private void Awake()
		{
			battleMoveChooser.OnStartSelection += EnableTextAndFill;
			battleMoveChooser.OnHighlightMove += MoveChanged;
			battleMoveChooser.OnEndSelection += DisableText;
		}

		private void OnDestroy()
		{
			battleMoveChooser.OnStartSelection -= EnableTextAndFill;
			battleMoveChooser.OnHighlightMove -= MoveChanged;
			battleMoveChooser.OnEndSelection -= DisableText;
		}

		private void EnableTextAndFill(BattleUnit unit)
		{
			battleText.SetTextAndActive("");
		}

		private void MoveChanged(BattleMove move)
		{
			battleText.SetText(move == null ? "" : move.MoveDescription);
		}

		private void DisableText()
		{
			battleText.SetActive(false);
		}
	}
}