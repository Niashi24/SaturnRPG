using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleAttackManager : MonoBehaviour
	{
		public readonly AsyncEvent<BattleAttack, BattleContext> OnAttack = new();

		public async UniTask<TurnOutcome> ProcessAttacks(BattleUnitManager unitManager, BattleContext context)
		{
			var sortedUnits = unitManager.ActiveUnits
				.OrderByDescending(x => x.SelectionPriority)
				.ThenBy(x => unitManager.ActiveUnits.IndexOf(x))
				.ToList();

			List<BattleAttack> attackStack = new(sortedUnits.Count);
			Dictionary<BattleUnit, BattleAttack> unitToAttack = new(sortedUnits.Count);
			HashSet<BattleUnit> exhaustedUnits = new(sortedUnits.Count);
			
			BattleUnit currentUnit;
			while ((currentUnit = sortedUnits.FirstWhere(x => !exhaustedUnits.Contains(x) && x.CanAttack())) != default)
			{
				BattleAttack attack;
				if (unitToAttack.ContainsKey(currentUnit))
					attack = await currentUnit.RedoAttackChoice(unitToAttack[currentUnit], context);
				else
					attack = await currentUnit.ChooseAttack(context);

				if (attack == null)  // Go back and re-do previous attack
				{
					if (attackStack.Count == 0) continue;

					var previousAttack = attackStack[^1];
					attackStack.RemoveAt(attackStack.Count - 1);
					// Assumption: moves cannot share units/use units already used
					foreach (var unit in previousAttack.GetExhaustedUnits(context))
						exhaustedUnits.Remove(unit);
				}
				else
				{
					attackStack.Add(attack);
					foreach (var unit in attack.GetExhaustedUnits(context))
						exhaustedUnits.Add(unit);
					unitToAttack[currentUnit] = attack;
				}
			}

			var sortedAttacks = attackStack
				.OrderBy(x => x.MoveBase.Priority)
				.ThenBy(x => attackStack.IndexOf(x));

			foreach (var attack in sortedAttacks)
			{
				if (attack.CanBeUsed())
				{
					await OnAttack.Invoke(attack, context);
					await attack.PlayAttack(context);
				}
				else  // Attack failed, try to fix it
				{
					// Assumption: The user in the attack is the one that chose it.
					var newAttack = attack.User.FixAttack(attack, context);
					if (newAttack.CanBeUsed())
					{
						await OnAttack.Invoke(attack, context);
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