using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class PartyMemberBattleUnit : BattleUnit
	{
		private PartyMember _partyMember;
		
		private int _HP;
		public override int HP => _HP;
		private int _MP;
		public override int MP => _MP;
		public override string Name => _partyMember != null ? _partyMember.name : "???";

		public override List<StatusCondition> StatusConditions { get; } = new();

		public override BattleStats BaseStats => _partyMember.Stats;

		public override bool CanAttack()
		{
			return true;
		}

		public override void SetPartyMember(PartyMember partyMember)
		{
			_partyMember = partyMember;
		}

		public override BattleStats GetBattleStats()
		{
			BattleStats outputStats = BaseStats;

			foreach (var statusCondition in StatusConditions)
				outputStats = statusCondition.ProcessStats(outputStats);

			return outputStats;
		}

		public override List<BattleMove> GetAvailableMoves(PartyMemberBattleUnit user, BattleContext context)
		{
			return new List<BattleMove>();
		}

		public override async UniTask<BattleAttack> ChooseAttack(BattleContext context)
		{
			return await _partyMember.BattleAttackChooser.ChooseAttack(context, this);
		}

		public override async UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition)
		{
			StatusConditions.Add(statusCondition);
			await statusCondition.OnAddCondition(context, this);
		}

		public override async UniTask TickStatusConditions(BattleContext context)
		{
			foreach (var statusCondition in StatusConditions)
				await statusCondition.Tick(context, this);
		}

		public override async UniTask DealDamage(int damage)
		{
			int oldHealth = _HP;
			_HP = (int)Mathf.Clamp(_HP - damage, 0, GetBattleStats().HP);
			// Do Hit Animation
			await OnHPChange.Invoke(_HP, oldHealth);
		}

		public override async UniTask UseMP(int mp)
		{
			int oldMP = _MP;
			_MP = (int)Mathf.Clamp(_MP - mp, 0, GetBattleStats().MP);
			// Do MP Animations
			await OnMPChange.Invoke(_MP, oldMP);
		}
	}
}