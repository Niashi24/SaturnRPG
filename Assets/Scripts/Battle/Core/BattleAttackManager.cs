using System.Linq;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleAttackManager : MonoBehaviour
	{
		public readonly AsyncEvent<BattleAttack> OnAttack = new();

		public async UniTask<TurnOutcome> ProcessAttacks(BattleUnitManager unitManager, BattleContext context)
		{
			BattleUnit[] sortedUnits = unitManager.ActiveUnits.OrderByDescending(x => x.SelectionPriority).ToArray();

			BattleAttack[] attacks = new BattleAttack[sortedUnits.Length];
			for (int i = 0; i < sortedUnits.Length; i++)
			{
				BattleAttack attack;
				if (attacks[i] == null)  // Haven't selected an attack yet
					attack = await sortedUnits[i].ChooseAttack(context);
				else  // Already selected an attack, redo attack choice
					attack = await sortedUnits[i].RedoAttackChoice(attacks[i], context);
				
				if (attack == null)
					i -= 2;  // Go back to previous (-2 b/c i++)
				else
					attacks[i] = attack;
			}

			foreach (var attack in attacks)
			{
				if (attack.CanBeUsed())
				{
					await OnAttack.Invoke(attack);
					await attack.PlayAttack(context);
				}
				else  // Attack failed, try to fix it
				{
					// Assumption: The user in the attack is the one that chose it.
					var newAttack = attack.User.FixAttack(attack, context);
					if (newAttack.CanBeUsed())
					{
						await OnAttack.Invoke(attack);
						await attack.PlayAttack(context);
					}
				}
			}

			if (context.PlayerUnitManager.AllUnitsDown())
				return TurnOutcome.PlayerLost;
			if (context.EnemyUnitManager.AllUnitsDown())
				return TurnOutcome.PlayerWon;
			return TurnOutcome.Continue;
		}
	}
}