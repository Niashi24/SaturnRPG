using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class StatusCondition : ScriptableObject
	{
		public abstract BattleStats ProcessStats(BattleStats stats);
	}
}