using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class AllUnit : BattleUnit
	{
		public override int HP => 0;
		public override int MP => 0;
		
		[SerializeField] private new string name;
		public override string Name => name;
		
		public override List<StatusCondition> StatusConditions => new ();
		public override BattleStats BaseStats => new BattleStats();

		private List<BattleUnit> _activeUnits;

		public void SetActiveUnits(List<BattleUnit> ActiveUnits)
		{
			_activeUnits = ActiveUnits;
		}

		public override void SetPartyMember(PartyMember partyMember)
		{
			Debug.LogError("Error! Tried to set party member of AllUnit.", this);
		}

		public override bool CanAttack()
		{
			Debug.LogError("Error! Tried to check if AllUnit can attack.", this);
			return false;
		}

		public override BattleStats GetBattleStats()
		{
			Debug.LogError("Error! Tried to get battle stats of AllUnit", this);
			return new BattleStats();
		}

		public override List<BattleMove> GetAvailableMoves(PartyMemberBattleUnit user, BattleContext context)
		{
			Debug.LogError("Error! Tried to get available moves of AllUnit.", this);
			return new List<BattleMove>();
		}

		public override async UniTask<BattleAttack> ChooseAttack(BattleContext context)
		{
			await UniTask.CompletedTask;
			return new BattleAttack();
		}

		public override async UniTask TickStatusConditions(BattleContext context)
		{
			Debug.LogError("Error! Tried to tick status conditions of AllUnit", this);
			await UniTask.CompletedTask;
			return;
		}

		public override async UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition)
		{
			if (_activeUnits == null)
			{
				Debug.LogError("Active Units not set in All Unit", this);
				await UniTask.CompletedTask;
				return;
			}

			await UniTask.WhenAll(_activeUnits.Select(x => x.AddStatusCondition(context, statusCondition)));
		}

		public override async UniTask DealDamage(int damage)
		{
			if (_activeUnits == null)
			{
				Debug.LogError("Active Units not set in All Unit", this);
				await UniTask.CompletedTask;
				return;
			}

			await UniTask.WhenAll(_activeUnits.Select(x => x.DealDamage(damage)));
		}

		public override async UniTask UseMP(int mp)
		{
			if (_activeUnits == null)
			{
				Debug.LogError("Active Units not set in All Unit", this);
				await UniTask.CompletedTask;
				return;
			}

			await UniTask.WhenAll(_activeUnits.Select(x => x.UseMP(mp)));
		}
	}
}