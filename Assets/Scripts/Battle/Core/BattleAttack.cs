using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle
{
	public class BattleAttack
	{
		public BattleMove MoveBase;
		public BattleUnit User;
		public ITargetable Target;
		public BattleStats Stats;

		public async UniTask PlayAttack(BattleContext context)
		{
			await MoveBase.PlayMove(context, this);
		}

		public bool CanBeUsed()
		{
			if (MoveBase == null) return false;
			if (User == null) return false;
			if (Target == null) return false;
			if (!User.CanAttack()) return false;

			return Target.CanBeAttacked();
		}

		public IEnumerable<BattleUnit> GetExhaustedUnits(BattleContext context)
			=> MoveBase == null ? Enumerable.Empty<BattleUnit>() : MoveBase.GetExhaustedUnits(this, context);
	}
}