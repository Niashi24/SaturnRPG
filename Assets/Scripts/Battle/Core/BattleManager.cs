	using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SaturnRPG.Battle
{
	public class BattleManager : MonoSingleton<BattleManager>
	{
		public readonly AsyncEvent<BattleContext> OnBattleStart = new();
		public readonly AsyncEvent<BattleState> OnBattleStateChange = new();
		public readonly AsyncEvent OnBattleWon = new(), OnBattleLost = new(), OnBattleQuit = new();
		public readonly AsyncEvent<int> OnTurnStart = new();
		
		public readonly BattleContext BattleContext = new();
		
		[SerializeField, Required]
		private BattleUnitManager playerUnitManager, enemyUnitManager;
		
		[SerializeField, Required]
		private BattleAttackManager battleAttackManager;

		[SerializeField, Required]
		private BattleCamera battleCamera;

		[SerializeField, Required]
		private BattleText battleText;

		[ShowInInspector, ReadOnly]
		public BattleState BattleState { get; private set; } = BattleState.End;
		[ShowInInspector, ReadOnly]
		public int TurnCount { get; private set; }

		[Button]
		[DisableInEditorMode]
		public async UniTask StartBattle(BattleParty playerParty, BattleParty enemyParty)
		{
			if (BattleState != BattleState.End)
				return;

			BattleContext.BattleManager = this;
			BattleContext.BattleCamera = battleCamera;
			BattleContext.PlayerParty = playerParty;
			BattleContext.PlayerUnitManager = playerUnitManager;
			BattleContext.EnemyParty = enemyParty;
			BattleContext.EnemyUnitManager = enemyUnitManager;
			BattleContext.BattleText = battleText;
			BattleContext.BattleCancellationToken = this.GetCancellationTokenOnDestroy();

			playerUnitManager.InitializeBattleUnits(playerParty);
			enemyUnitManager.InitializeBattleUnits(enemyParty);

			TurnCount = 1;
			
			await BattleAsync();
		}

		private async UniTask ChangeState(BattleState state)
		{
			this.BattleState = state;
			Debug.Log($"Changed to {state} state");
			await OnBattleStateChange.Invoke(state);
		}

		private async UniTask BattleAsync()
		{
			await OnBattleStart.Invoke(BattleContext);
			await ChangeState(BattleState.Start);

			// awaits the given async function, gets the turn outcome from it
			// if the battle should end, returns true, otherwise returns false
			async UniTask<bool> AwaitTurnOutcome(UniTask<TurnOutcome> turnOutcome)
			{
				switch (await turnOutcome)
				{
					case TurnOutcome.Continue:
						return false;
					case TurnOutcome.PlayerWon:
						await EndBattle(true);
						return true;
					case TurnOutcome.PlayerLost:
						await EndBattle(false);
						return true;
					default:
						return false;
				}
			}

			while (!playerUnitManager.AllUnitsDown() && !enemyUnitManager.AllUnitsDown())
			{
				await OnTurnStart.Invoke(TurnCount);
				
				await ChangeState(BattleState.PlayerTurn);

				if (await AwaitTurnOutcome(battleAttackManager.ProcessAttacks(playerUnitManager, BattleContext)))
					return;

				await ChangeState(BattleState.EnemyTurn);

				if (await AwaitTurnOutcome(battleAttackManager.ProcessAttacks(enemyUnitManager, BattleContext)))
					return;

				if (await AwaitTurnOutcome(enemyUnitManager.TickStatusConditions(BattleContext)))
					return;

				if (await AwaitTurnOutcome(playerUnitManager.TickStatusConditions(BattleContext)))
					return;

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