using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle.Moves
{
	public class RunTeamMove : BattleMoveComponent
	{
		public override UniTask PlayAttack(BattleContext context, BattleAttack attack)
		{
			// Call BattleManager to end battle
			return UniTask.CompletedTask;
		}

		public override bool CanBeUsed(BattleContext context, BattleUnit user)
		{
			var teamUnits = context.GetTeam(user).ActiveUnits;
			if (teamUnits.Count == 0) return false;
			return teamUnits[0] == user;
		}

		public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
		{
			return new List<ITargetable>() { user };
		}

		public override IEnumerable<BattleUnit> GetExhaustedUnitsOfAttack(BattleAttack attack, BattleContext context)
		{
			return context.GetTeam(attack.User).ActiveUnits;
		}
	}
}