using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class BattleMoveComponent : MonoBehaviour
	{
		public abstract UniTask PlayAttack(BattleContext context, BattleAttack attack);

		public abstract bool CanBeUsed(BattleContext context, PartyMemberBattleUnit user);

		public abstract List<PartyMemberBattleUnit> GetTargetableUnits(PartyMemberBattleUnit user, BattleContext context);
	}
}