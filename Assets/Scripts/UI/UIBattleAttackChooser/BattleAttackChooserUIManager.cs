using System;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class BattleAttackChooserUIManager : MonoSingleton<BattleAttackChooserUIManager>
	{
		[SerializeField, Required]
		private UIBattleMoveChooser battleMoveChooser;

		[SerializeField, Required]
		private BattleTargetChooserUI battleTargetChooser;

		public readonly AsyncEvent<BattleMove> OnChooseMove = new();
		public readonly AsyncEvent<BattleAttack> OnChooseAttack = new();

		public async UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit)
		{
			ResetUI();
			return await WaitForAttack(context, unit);
		}

		public async UniTask<BattleAttack> RedoChoiceSelection(BattleContext context, BattleUnit unit, BattleAttack previous)
		{
			ResetUI();
			battleMoveChooser.SetSelection(previous.MoveBase);
			battleTargetChooser.SetSelection(previous.Target);
			
			// Just try to reselect target
			var target = await battleTargetChooser.ChooseTarget(context, unit, previous.MoveBase);
			
			if (target == null)  // Didn't work, just choose attack normally
				return await WaitForAttack(context, unit);
			
			ResetUI();
			return new BattleAttack()
			{
				MoveBase = previous.MoveBase,
				Stats = previous.Stats,
				Target = target,
				User = previous.User
			};
		}

		private void ResetUI()
		{
			battleTargetChooser.ResetSelection();
			battleMoveChooser.ResetSelection();
		}

		private async UniTask<BattleAttack> WaitForAttack(BattleContext context, BattleUnit unit)
		{
			while (true)
			{
				BattleMove move = await battleMoveChooser.ChooseMove(context, unit);
				if (move == null) return null;  // cancel selection

				ITargetable target = await battleTargetChooser.ChooseTarget(context, unit, move);
				if (target == null) continue;  // restart selection

				ResetUI();

				return new BattleAttack()
				{
					MoveBase = move,
					Stats = move.GetMoveStats(unit, target, context),
					Target = target,
					User = unit
				};
			}
		}
	}
}