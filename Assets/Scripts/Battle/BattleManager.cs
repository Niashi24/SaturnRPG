using System;
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
		public readonly AsyncEvent OnBattleWon = new(), OnBattleLost = new(), OnBattleQuit = new();
		public readonly AsyncEvent<int> OnTurnStart = new();
		
		private readonly BattleContext _battleContext = new();
		
		[SerializeField, Required]
		private BattleUnitManager playerUnitManager, enemyUnitManager;
		
		[SerializeField, Required]
		private BattleAttackManager battleAttackManager;
		
		[ShowInInspector, ReadOnly]
		public BattleState BattleState { get; private set; }
		[ShowInInspector, ReadOnly]
		public int TurnCount { get; private set; }

		private void Start()
		{
			// BattleAsync().Forget();
		}

		[Button]
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

				if (await AwaitTurnOutcome(battleAttackManager.ProcessAttacks(playerUnitManager, _battleContext)))
					return;

				if (await AwaitTurnOutcome(battleAttackManager.ProcessAttacks(enemyUnitManager, _battleContext)))
					return;

				if (await AwaitTurnOutcome(enemyUnitManager.TickStatusConditions(_battleContext)))
					return;

				if (await AwaitTurnOutcome(playerUnitManager.TickStatusConditions(_battleContext)))
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