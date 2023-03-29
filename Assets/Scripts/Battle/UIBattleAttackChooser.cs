using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle
{
	public class UIBattleAttackChooser : BattleAttackChooser
	{
		public override async UniTask<BattleAttack> ChooseAttack(BattleContext context, PartyMemberBattleUnit unit)
		{
			await UniTask.CompletedTask;
			throw new System.NotImplementedException();
		}
	}
}