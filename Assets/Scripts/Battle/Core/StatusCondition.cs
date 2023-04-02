using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class StatusCondition : ScriptableObject
	{
		public abstract BattleStats ProcessStats(BattleStats stats);

		public abstract UniTask OnAddCondition(BattleContext context, BattleUnit unit);

		public abstract UniTask Tick(BattleContext context, BattleUnit unit);
	}
}