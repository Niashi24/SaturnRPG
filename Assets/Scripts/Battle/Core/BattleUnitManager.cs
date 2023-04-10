using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnitManager : MonoBehaviour
	{
		public event Action<List<BattleUnit>> OnSetActiveUnits;
		
		[SerializeField]
		private BattleUnit[] availableUnits;
		
		[field: SerializeField, Required]
		public AllUnit AllTargetable { get; private set; }

		[ShowInInspector, ReadOnly]
		public List<BattleUnit> ActiveUnits { get; private set; } = new();

		public List<BattleUnit> GetTargetableUnits()
		{
			List<BattleUnit> TargetableUnits = new();
			foreach (var unit in ActiveUnits)
				if (unit.CanBeAttacked())
					TargetableUnits.Add(unit);

			return TargetableUnits;
		}

		public List<BattleUnit> GetAttackingUnits()
		{
			List<BattleUnit> AttackingUnits = new();
			foreach (var unit in ActiveUnits)
				if (unit.CanAttack())
					AttackingUnits.Add(unit);
			
			return AttackingUnits;
		}

		public void InitializeBattleUnits(BattleParty battleParty)
		{
			ActiveUnits.Clear();
			for (int i = 0; i < battleParty.PartyMembers.Count && i < availableUnits.Length; i++)
			{
				availableUnits[i].SetPartyMember(battleParty.PartyMembers[i]);
				ActiveUnits.Add(availableUnits[i]);
			}
			
			AllTargetable.SetActiveUnits(ActiveUnits);
			
			OnSetActiveUnits?.Invoke(ActiveUnits);
		}

		public bool AllUnitsDown()
		{
			foreach (var unit in ActiveUnits)
			{
				if (unit.HP > 0) return false;
			}

			return true;
		}

		public async UniTask<TurnOutcome> TickStatusConditions(BattleContext context)
		{
			UniTask[] unitTasks = new UniTask[ActiveUnits.Count];
			for (int i = 0; i < ActiveUnits.Count; i++)
			{
				unitTasks[i] = ActiveUnits[i].TickStatusConditions(context);
			}

			await UniTask.WhenAll(unitTasks);

			if (context.PlayerUnitManager.AllUnitsDown())
				return TurnOutcome.PlayerLost;
			if (context.EnemyUnitManager.AllUnitsDown())
				return TurnOutcome.PlayerWon;
			return TurnOutcome.Continue;
		}
	}
}