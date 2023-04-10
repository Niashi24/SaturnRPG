using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnit : MonoBehaviour, ITargetable
	{
		[field: SerializeField, Required]
		public UnitVisual UnitVisual { get; private set; }
		
		public event Action<PartyMember> OnSetPartyMember;
		public readonly AsyncEvent<int, int> OnHPChange = new();
		public readonly AsyncEvent<int, int> OnMPChange = new();
		
		[ShowInInspector, ReadOnly]
		public PartyMember PartyMember { get; private set; }
		[ShowInInspector, ReadOnly]
		public int HP { get; private set; }
		[ShowInInspector, ReadOnly]
		public int MP { get; private set; }
		[ShowInInspector, ReadOnly]
		public string Name => PartyMember != null ? PartyMember.Name : "???";
		public int SelectionPriority => PartyMember != null ? PartyMember.BattleAttackChooser.SelectionPriority : 0;

		[ShowInInspector, ReadOnly]
		public List<StatusCondition> StatusConditions { get; private set; } = new();

		public BattleStats BaseStats => PartyMember.Stats;

		public bool CanAttack()
		{
			return HP > 0;
		}

		public bool CanBeAttacked()
		{
			return HP > 0;
		}

		public void SetPartyMember(PartyMember partyMember)
		{
			PartyMember = partyMember;
			HP = partyMember.GetStartHP();
			MP = partyMember.GetStartMP();
			
			OnSetPartyMember?.Invoke(PartyMember);
		}

		public BattleStats GetBattleStats()
		{
			BattleStats outputStats = BaseStats;

			foreach (var statusCondition in StatusConditions)
				outputStats = statusCondition.ProcessStats(outputStats);

			return outputStats;
		}

		public List<BattleMove> GetAvailableMoves(BattleContext context)
		{
			List<BattleMove> availableMoves = new();
			if (PartyMember != null)
				availableMoves.AddRange(PartyMember.Moves);
			// TODO: If player is first in party, add "Run" move?
			return availableMoves;
		}

		public UniTask<BattleAttack> ChooseAttack(BattleContext context) 
			=> PartyMember.BattleAttackChooser.ChooseAttack(context, this);

		public UniTask<BattleAttack> RedoAttackChoice(BattleAttack previous, BattleContext context)
			=> PartyMember.BattleAttackChooser.RedoChoiceSelection(context, this, previous);

		public BattleAttack FixAttack(BattleAttack former, BattleContext context)
			=> PartyMember.BattleAttackChooser.FixAttack(former, context);

		public async UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition)
		{
			StatusConditions.Add(statusCondition);
			await statusCondition.OnAddCondition(context, this);
		}

		public async UniTask TickStatusConditions(BattleContext context)
		{
			foreach (var statusCondition in StatusConditions)
				await statusCondition.Tick(context, this);
		}

		public async UniTask DealDamage(int damage)
		{
			int oldHealth = HP;
			HP = Mathf.Clamp(HP - damage, 0, GetBattleStats().HP);
			// Do Hit Animation
			await OnHPChange.Invoke(HP, oldHealth);
		}

		public async UniTask UseMP(int mp)
		{
			int oldMP = MP;
			MP = Mathf.Clamp(MP - mp, 0, GetBattleStats().MP);
			// Do MP Animations
			await OnMPChange.Invoke(MP, oldMP);
		}
	}
}