using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle.Moves
{
	public class DoNothingMove : BattleMoveComponent
	{
		public override async UniTask PlayAttack(BattleContext context, BattleAttack attack)
		{
			Debug.Log($"{attack.User.Name} did nothing!");
			await UniTask.Delay(1000, cancellationToken: context.BattleCancellationToken);
		}

		public override bool CanBeUsed(BattleContext context, BattleUnit user)
			=> true;

		public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
		{
			return new List<ITargetable>() { user };
		}
	}
}