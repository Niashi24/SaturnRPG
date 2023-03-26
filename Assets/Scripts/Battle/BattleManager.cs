using System;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SaturnRPG.Battle
{
	public class BattleManager : MonoBehaviour
	{
		[SerializeField] public AsyncEvent<BattleContext> OnBattleStart;

		private BattleContext _battleContext = new();
		
		[SerializeField] [Required] private BattleUnitManager playerUnitManager;

		[SerializeField] [Required] private BattleUnitManager enemyUnitManager;

		private void Start()
		{
			BattleAsync();
		}

		public void StartBattle(BattleParty playerParty, BattleParty enemyParty)
		{
			_battleContext.PlayerParty = playerParty;
			_battleContext.PlayerUnitManager = playerUnitManager;
			_battleContext.EnemyParty = enemyParty;
			_battleContext.EnemyUnitManager = enemyUnitManager;
		}

		private async void BattleAsync()
		{
			Debug.Log("Test Started");
			await OnBattleStart.Invoke(_battleContext);
			Debug.Log("Test Finished");
		}
	}
}