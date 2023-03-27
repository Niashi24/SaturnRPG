using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class BattleMoveComponent : MonoBehaviour
	{
		public abstract UniTask PlayAttack(BattleContext context, BattleAttack attack);
	}
}