using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle.Moves
{
	public class TestMove : BattleMoveComponent
	{
		public override async UniTask PlayAttack(BattleContext context, BattleAttack attack)
		{
			BattleStats userStats = attack.Stats;
			BattleStats enemyStats = attack.Target.GetBattleStats();
			
			Debug.Log($"{attack.User.Name} attacked {attack.Target.Name}!");
			await UniTask.Delay(1000);

			await attack.Target.DealDamage(userStats.Attack - enemyStats.Defense);
		}

		public override bool CanBeUsed(BattleContext context, BattleUnit user)
		{
			return true;
		}

		public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
		{
			var targetables = new List<ITargetable>();
			targetables.AddRange(context.GetEnemies(user).GetTargetableUnits());
			return targetables;
		}
	}
}