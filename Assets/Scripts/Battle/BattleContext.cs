﻿namespace SaturnRPG.Battle
{
	public class BattleContext
	{
		public BattleManager BattleManager;
		
		public BattleParty PlayerParty;
		public BattleParty EnemyParty;

		public BattleUnitManager PlayerUnitManager;
		public BattleUnitManager EnemyUnitManager;

		public BattleUnitManager GetEnemies(BattleUnit user)
		{
			if (PlayerUnitManager.ActiveUnits.Contains(user))
				return EnemyUnitManager;
			return PlayerUnitManager;
		}
	}
}