using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle.Moves
{
	public class DoNothingMove : BattleMoveComponent
	{
		public override UniTask PlayAttack(BattleContext context, BattleAttack attack)
		{
			Debug.Log($"{attack.User.Name} did nothing!");
			return UniTask.CompletedTask;
		}

		public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
		{
			return new() { user };
		}

		public override bool ShouldAutoTargetIfOnlyOne => true;
	}
}