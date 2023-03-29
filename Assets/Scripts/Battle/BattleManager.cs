﻿using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SaturnRPG.Battle
{
	public class BattleManager : MonoBehaviour
	{
		public readonly AsyncEvent<BattleContext> OnBattleStart = new();
		public readonly AsyncEvent<BattleState> OnBattleStateChange = new();
		public readonly AsyncEvent OnBattleWon = new(), OnBattleLost = new();
		public readonly AsyncEvent<int> OnTurnStart = new();
		
		private readonly BattleContext _battleContext = new();
		
		[SerializeField, Required] private BattleUnitManager playerUnitManager, enemyUnitManager;
		
		[ShowInInspector, ReadOnly]
		public BattleState BattleState { get; private set; }
		[ShowInInspector, ReadOnly]
		public int TurnCount { get; private set; }

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

			TurnCount = 1;
			
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
				await OnTurnStart.Invoke(TurnCount);
				await ChangeState(BattleState.PlayerTurn);

				foreach (var unit in playerUnitManager.ActiveUnits)
				{
					if (!unit.CanAttack()) continue;

					var attack = await unit.ChooseAttack(_battleContext);
					
					await attack.PlayAttack(_battleContext);

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

				foreach (var unit in enemyUnitManager.ActiveUnits)
				{
					if (!unit.CanAttack()) continue;

					var attack = await unit.ChooseAttack(_battleContext);
					
					await attack.PlayAttack(_battleContext);

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

				var tickAllies = playerUnitManager.ActiveUnits.Select(x => x.TickStatusConditions(_battleContext));
				var tickEnemies = enemyUnitManager.ActiveUnits.Select(x => x.TickStatusConditions(_battleContext));

				await UniTask.WhenAll(tickAllies);
				await UniTask.WhenAll(tickEnemies);

				TurnCount++;
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