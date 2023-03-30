using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnitManager : MonoBehaviour
	{
		[SerializeField]
		private BattleUnit[] availableUnits;
		
		[field: SerializeField, Required]
		public AllUnit AllTargetable { get; private set; }

		public List<BattleUnit> ActiveUnits { get; private set; } = new();

		public void InitializeBattleUnits(BattleParty battleParty)
		{
			ActiveUnits.Clear();
			for (int i = 0; i < battleParty.PartyMembers.Count && i < availableUnits.Length; i++)
			{
				availableUnits[i].SetPartyMember(battleParty.PartyMembers[i]);
			}
			
			AllTargetable.SetActiveUnits(ActiveUnits);
		}

		public bool AllUnitsDown()
		{
			if (ActiveUnits.Count == 0) return true;

			foreach (var unit in ActiveUnits)
			{
				if (unit.HP > 0) return false;
			}

			return true;
		}
	}
}