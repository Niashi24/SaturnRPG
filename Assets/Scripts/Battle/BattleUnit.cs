using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class BattleUnit : MonoBehaviour
	{
		public Action<PartyMemberBattleUnit, PartyMember> OnSetPartyMember;
		public readonly AsyncEvent<int, int> OnHPChange = new();
		public readonly AsyncEvent<int, int> OnMPChange = new();
		
		// public abstract PartyMember PartyMember { get; }
		public abstract int HP { get; }
		public abstract int MP { get; }
		public abstract string Name { get; }
		public abstract List<StatusCondition> StatusConditions { get; }
		public abstract BattleStats BaseStats { get; }
		public abstract void SetPartyMember(PartyMember partyMember);
		public abstract bool CanAttack();
		public abstract BattleStats GetBattleStats();
		public abstract List<BattleMove> GetAvailableMoves(PartyMemberBattleUnit user, BattleContext context);
		public abstract UniTask<BattleAttack> ChooseAttack(BattleContext context);
		public abstract UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition);
		public abstract UniTask TickStatusConditions(BattleContext context);
		public abstract UniTask DealDamage(int damage);
		public abstract UniTask UseMP(int mp);
	}
}