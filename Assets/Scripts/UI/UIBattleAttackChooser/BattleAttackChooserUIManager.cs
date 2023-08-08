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

		public readonly AsyncEvent<BattleUnit, BattleContext> OnStartChooseAttack = new();
		public readonly AsyncEvent<BattleMove> OnChooseMove = new();
		public readonly AsyncEvent<BattleAttack> OnChooseAttack = new();

		public async UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit)
		{
			await OnStartChooseAttack.Invoke(unit, context);
			return await WaitForAttack(context, unit);
		}

		public async UniTask<BattleAttack> RedoChoiceSelection(BattleContext context, BattleUnit unit, BattleAttack previous)
		{
			await OnStartChooseAttack.Invoke(unit, context);
			
			// Just try to reselect target
			var target = await battleTargetChooser.ReChooseTarget(context, unit, previous.MoveBase, previous.Target);
			
			if (target == null)  // Didn't work, just choose attack normally
				return await WaitForAttack(context, unit, previous.MoveBase);
			
			// Worked! Return the new Battle Attack
			var attack = new BattleAttack()
			{
				MoveBase = previous.MoveBase,
				Stats = previous.Stats,
				Target = target,
				User = previous.User
			};
			await OnChooseAttack.Invoke(attack);
			return attack;
		}

		private async UniTask<BattleAttack> WaitForAttack(BattleContext context, BattleUnit unit, BattleMove previous = null)
		{
			BattleMove selectedMove = previous;
			while (true)
			{
				if (selectedMove == null)
					selectedMove = await battleMoveChooser.ChooseMove(context, unit);
				else  // selected move then canceled target selection
					selectedMove = await battleMoveChooser.RedoMoveChoice(context, unit, selectedMove);
						
				if (selectedMove == null) return null;  // cancel selection

				await OnChooseMove.Invoke(selectedMove);

				ITargetable target = await battleTargetChooser.ChooseTarget(context, unit, selectedMove);
				if (target == null) continue;  // restart selection

				var attack = new BattleAttack()
				{
					MoveBase = selectedMove,
					Stats = selectedMove.GetMoveStats(unit, target, context),
					Target = target,
					User = unit
				};

				await OnChooseAttack.Invoke(attack);
				
				return attack;
			}
		}
	}
}