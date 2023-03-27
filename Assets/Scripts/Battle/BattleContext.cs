namespace SaturnRPG.Battle
{
	public class BattleContext
	{
		public BattleManager BattleManager;
		
		public BattleParty PlayerParty;
		public BattleParty EnemyParty;

		public BattleUnitManager PlayerUnitManager;
		public BattleUnitManager EnemyUnitManager;
	}
}