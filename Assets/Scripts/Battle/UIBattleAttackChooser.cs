using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Battle
{
	public class UIBattleAttackChooser : BattleAttackChooser
	{
		public override async UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit)
		{
			await UniTask.CompletedTask;
			throw new System.NotImplementedException();
		}
	}
}