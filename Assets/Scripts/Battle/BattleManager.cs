using System;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.PlayerLoop;

namespace SaturnRPG.Battle
{
	public class BattleManager : MonoBehaviour
	{
		[SerializeField] public AsyncEvent<BattleContext> OnBattleStart;

		[SerializeField] public AsyncEvent<BattleState> OnBattleStateChange;

		[SerializeField] public AsyncEvent OnBattleWon, OnBattleLost;

		private readonly BattleContext _battleContext = new();
		
		[SerializeField, Required] private BattleUnitManager playerUnitManager;

		[SerializeField, Required] private BattleUnitManager enemyUnitManager;
		
		[ShowInInspector, ReadOnly]
		public BattleState BattleState { get; private set; }

		private void Start()
		{
			BattleAsync().Forget();
		}

		public void StartBattle(BattleParty playerParty, BattleParty enemyParty)
		{
			_battleContext.BattleManager = this;
			_battleContext.PlayerParty = playerParty;
			_battleContext.PlayerUnitManager = playerUnitManager;
			_battleContext.EnemyParty = enemyParty;
			_battleContext.EnemyUnitManager = enemyUnitManager;
			
			playerUnitManager.InitializeBattleUnits(playerParty);
			enemyUnitManager.InitializeBattleUnits(enemyParty);
			
			BattleAsync().Forget();
		}

		private async UniTask ChangeState(BattleState state)
		{
			this.BattleState = state;
			await OnBattleStateChange.Invoke(state);
		}

		private async UniTaskVoid BattleAsync()
		{
			await OnBattleStart.Invoke(_battleContext);
			await ChangeState(BattleState.Start);

			while (!playerUnitManager.AllUnitsDown() && !enemyUnitManager.AllUnitsDown())
			{
				await ChangeState(BattleState.PlayerTurn);

				foreach (var unit in playerUnitManager.ActiveUnits)
				{
					if (!unit.CanAttack) continue;

					var attack = await unit.ChooseAttack(_battleContext);

					if (playerUnitManager.AllUnitsDown())
					{
						await EndBattle(false);
						return;
					}

					if (enemyUnitManager.AllUnitsDown())
					{
						await EndBattle(true);
						return;
					}
				}
			}
		}

		private async UniTask EndBattle(bool playerWon)
		{
			await ChangeState(BattleState.End);
			if (playerWon)
				await OnBattleWon.Invoke();
			else
				await OnBattleLost.Invoke();
		}
	}
}