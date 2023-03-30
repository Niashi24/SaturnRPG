using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle
{
	public interface ITargetable
	{
		int HP { get; }
		int MP { get; }
		string Name { get; }
		List<StatusCondition> StatusConditions { get; }
		BattleStats BaseStats { get; }

		bool CanBeAttacked();
		BattleStats GetBattleStats();
		UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition);
		UniTask DealDamage(int damage);
		UniTask UseMP(int mp);
	}
}