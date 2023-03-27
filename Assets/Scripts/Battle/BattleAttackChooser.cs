using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class BattleAttackChooser : ScriptableObject
	{
		[field: SerializeField]
		[Tooltip("Higher priority choosers get called first (AI goes before Player).")]
		public int SelectionPriority { get; private set; } = 0;
		
		public abstract UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit);
	}
}