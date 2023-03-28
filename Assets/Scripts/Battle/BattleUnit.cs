using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnit : MonoBehaviour
	{
		[Header("Events")] public Action<BattleUnit, PartyMember> OnSetPartyMember;

		// first int is new, second int is old
		public AsyncEvent<int, int> OnHPChange, OnMPChange;

		public PartyMember PartyMember { get; private set; }

		public int HP { get; private set; }
		public int MP { get; private set; }
		
		public string Name { get; private set; }

		public bool CanAttack { get; private set; } = true;

		public List<StatusCondition> StatusConditions { get; private set; } = new();

		public BattleStats BaseStats => PartyMember.Stats;

		public void SetPartyMember(PartyMember partyMember)
		{
			this.PartyMember = partyMember;
		}

		public BattleStats GetBattleStats()
		{
			BattleStats outputStats = BaseStats;

			foreach (var statusCondition in StatusConditions)
				outputStats = statusCondition.ProcessStats(outputStats);

			return outputStats;
		}

		public List<BattleMove> GetAvailableMoves(BattleUnit user, BattleContext context)
		{
			return new List<BattleMove>();
		}

		public async UniTask<BattleAttack> ChooseAttack(BattleContext context)
		{
			return await PartyMember.BattleAttackChooser.ChooseAttack(context, this);
		}

		public async UniTask DealDamage(int damage)
		{
			int oldHealth = HP;
			HP = (int)Mathf.Clamp(HP - damage, 0, GetBattleStats().HP);
			// Do Hit Animation
			await OnHPChange.Invoke(HP, oldHealth);
		}

		public async UniTask UseMP(int mp)
		{
			int oldMP = MP;
			MP = (int)Mathf.Clamp(MP - mp, 0, GetBattleStats().MP);
			// Do MP Animations
			await OnMPChange.Invoke(MP, oldMP);
		}
	}
}