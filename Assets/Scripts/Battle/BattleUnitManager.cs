using System.Collections.Generic;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnitManager : MonoBehaviour
	{
		[SerializeField] private BattleUnit[] availableUnits;
		
		public List<BattleUnit> ActiveUnits { get; private set; }

		public void InitializeBattleUnits(BattleParty battleParty)
		{
			
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