using System.Collections.Generic;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnitManager : MonoBehaviour
	{
		[SerializeField] private PartyMemberBattleUnit[] availableUnits;

		public List<PartyMemberBattleUnit> ActiveUnits { get; private set; } = new();

		public void InitializeBattleUnits(BattleParty battleParty)
		{
			ActiveUnits.Clear();
			for (int i = 0; i < battleParty.PartyMembers.Count && i < availableUnits.Length; i++)
			{
				availableUnits[i].SetPartyMember(battleParty.PartyMembers[i]);
			}
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