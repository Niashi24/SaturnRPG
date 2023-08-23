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

		public virtual IEnumerable<BattleUnit> GetExhaustedUnitsOfAttack(BattleAttack user, BattleContext context)
		{
			yield return user.User;
		}

		public virtual BattleStats GetMoveStats(BattleUnit user, ITargetable target, BattleContext context)
		{
			return user.GetBattleStats();
		}

		/// <summary>
		/// If GetTargetables only returns one (valid) option,
		/// whether the move should auto-target that one enemy
		/// is determined by the below.
		/// Should be used only if target is always known beforehand
		/// (ex. targeting oneself or the AllUnit of the enemy team).
		/// Needs to be implemented in each attack chooser ;-;
		/// </summary>
		public virtual bool ShouldAutoTargetIfOnlyOne => false;
	}
}