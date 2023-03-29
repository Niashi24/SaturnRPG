using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle
{
	public class BattleAttack
	{
		public BattleMove MoveBase;
		public PartyMemberBattleUnit User;
		public PartyMemberBattleUnit Target;
		public BattleStats Stats;

		public async UniTask PlayAttack(BattleContext context)
		{
			await MoveBase.PlayMove(context, this);
		}
	}
}