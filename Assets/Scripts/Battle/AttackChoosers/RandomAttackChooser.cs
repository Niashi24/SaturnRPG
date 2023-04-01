using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Attack Chooser/Random Attack Chooser")]
	public class RandomAttackChooser : BattleAttackChooser
	{
		[SerializeField, Required]
		private BattleMove defaultMove;

		[SerializeField, Required]
		private BattleMove doNothingMove;
		
		public override async UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit)
		{
			List<BattleMove> usableMoves = unit.GetAvailableMoves(context);
			usableMoves.Shuffle();
			for (int i = 0; i < usableMoves.Count; i++)
			{
				var targetables = usableMoves[i].GetTargetables(unit, context);
				targetables.Shuffle();
				for (int j = 0; j < targetables.Count; j++)
				{
					if (targetables[i].CanBeAttacked())
						return new BattleAttack()
						{
							MoveBase = usableMoves[i],
							Stats = unit.GetBattleStats(),
							Target = targetables[i],
							User = unit
						};
				}
			}
			// try default move
			var defaultTargetables = defaultMove.GetTargetables(unit, context);
			defaultTargetables.Shuffle();
			for (int i = 0; i < defaultTargetables.Count; i++)
			{
				if (defaultTargetables[i].CanBeAttacked())
					return new BattleAttack()
					{
						MoveBase = usableMoves[i],
						Stats = unit.GetBattleStats(),
						Target = defaultTargetables[i],
						User = unit
					};
			}
			
			await UniTask.CompletedTask;
			// Could not make an attack, just make something up
			return new BattleAttack()
			{
				MoveBase = doNothingMove,
				User = unit,
				Target = unit,
				Stats = new()
			};
		}

		public override async UniTask<BattleAttack> RedoChoiceSelection(BattleContext context, BattleUnit unit, BattleAttack former)
		{
			await UniTask.CompletedTask;
			return former;
		}
	}
}