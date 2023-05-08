using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class BattleMoveComponent : MonoBehaviour
	{
		public abstract UniTask PlayAttack(BattleContext context, BattleAttack attack);

		public abstract bool CanBeUsed(BattleContext context, BattleUnit user);

		public abstract List<ITargetable> GetTargetables(BattleUnit user, BattleContext context);

		public virtual BattleStats GetMoveStats(BattleUnit user, ITargetable target, BattleContext context)
		{
			return user.GetBattleStats();
		}
	}
}