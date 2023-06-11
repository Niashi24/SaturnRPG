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
			// Continually get next usable unit until all units are used
			while ((currentUnit = sortedUnits.FirstWhere(x => !exhaustedUnits.Contains(x) && x.CanAttack())) != null)
			{
				BattleAttack attack;
				if (unitToAttack.ContainsKey(currentUnit))
					attack = await currentUnit.RedoAttackChoice(unitToAttack[currentUnit], context);
				else
					attack = await currentUnit.ChooseAttack(context);

				if (attack == null)  // Go back and re-do previous attack
				{
					// Clear attack of current unit from memory if it exists
					unitToAttack.Remove(currentUnit);
					
					if (attackStack.Count == 0) continue;
					// Get and remove previous attack created
					var previousAttack = attackStack.PopEnd();
					// Assumption: moves cannot share units/use units already used
					// Free all the units used in that attack to move again
					foreach (var unit in previousAttack.GetExhaustedUnits(context))
						exhaustedUnits.Remove(unit);
				}
				else
				{
					// Add created attack to list of attacks
					attackStack.Add(attack);
					// Add each unit 
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
					await PlayAttack(attack, context);
				}
				else  // Attack failed, try to fix it
				{
					// Assumption: The user in the attack is the one that chose it.
					var newAttack = attack.User.FixAttack(attack, context);
					if (newAttack.CanBeUsed())
					{
						await PlayAttack(newAttack, context);
					}
				}
				
				if (context.PlayerUnitManager.AllUnitsDown())
					return TurnOutcome.PlayerLost;
				if (context.EnemyUnitManager.AllUnitsDown())
					return TurnOutcome.PlayerWon;
			}

			async UniTask PlayAttack(BattleAttack attack, BattleContext context)
			{
				await OnAttack.Invoke(attack, context);
				await attack.User.UseMP(attack.MoveBase.MPCost);
				await attack.PlayAttack(context);
			}

			if (context.PlayerUnitManager.AllUnitsDown())
				return TurnOutcome.PlayerLost;
			if (context.EnemyUnitManager.AllUnitsDown())
				return TurnOutcome.PlayerWon;
			return TurnOutcome.Continue;
		}
	}
}