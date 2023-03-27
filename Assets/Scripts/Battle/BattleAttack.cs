using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle
{
	public class BattleAttack
	{
		public BattleMove MoveBase;
		public BattleUnit User;
		public BattleUnit Target;

		public async UniTask PlayAttack(BattleContext context)
		{
			await MoveBase.PlayMove(context, this);
		}
	}
}