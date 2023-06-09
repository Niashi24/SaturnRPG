﻿using System.Threading;

namespace SaturnRPG.Battle
{
	public class BattleContext
	{
		public BattleManager BattleManager;
		public BattleCamera BattleCamera;
		
		public BattleParty PlayerParty;
		public BattleParty EnemyParty;

		public BattleUnitManager PlayerUnitManager;
		public BattleUnitManager EnemyUnitManager;

		public CancellationToken BattleCancellationToken;

		public BattleUnitManager GetEnemies(BattleUnit user)
		{
			if (PlayerUnitManager.ActiveUnits.Contains(user))
				return EnemyUnitManager;
			return PlayerUnitManager;
		}

		public BattleUnitManager GetTeam(BattleUnit user)
		{
			if (PlayerUnitManager.ActiveUnits.Contains(user))
				return PlayerUnitManager;
			return EnemyUnitManager;
		}
	}
}