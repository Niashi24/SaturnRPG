using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle.Moves
{
	public class DoNothingMove : BattleMoveComponent
	{
		public override UniTask PlayAttack(BattleContext context, BattleAttack attack)
			=> UniTask.CompletedTask;

		public override bool CanBeUsed(BattleContext context, BattleUnit user)
			=> true;

		public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
		{
			return new List<ITargetable>() { user };
		}
	}
}