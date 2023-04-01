using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class BattleAttackChooser : ScriptableObject
	{
		[field: SerializeField]
		[field: Tooltip("Higher priority choosers get called first (AI goes before Player).")]
		public int SelectionPriority { get; private set; } = 0;
		
		public abstract UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit);

		public abstract UniTask<BattleAttack> RedoChoiceSelection(BattleContext context, BattleUnit unit, BattleAttack former);

		public virtual BattleAttack FixAttack(BattleAttack former, BattleContext context)
		{
			if (!former.User.CanAttack())
				return former;
			
			if (!former.Target.CanBeAttacked() && former.Target is BattleUnit unit)
			{
				BattleUnitManager activeTeam;
				if (context.EnemyUnitManager.ActiveUnits.Contains(unit))
					activeTeam = context.EnemyUnitManager;
				else
					activeTeam = context.PlayerUnitManager;

				for (int i = 0; i < activeTeam.ActiveUnits.Count; i++)
				{
					if (!activeTeam.ActiveUnits[i].CanBeAttacked())
						continue;

					former.Target = activeTeam.ActiveUnits[i];
					return former;
				}
			}
			
			return former;
		}
	}
}