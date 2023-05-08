using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.UI;
using UnityEngine;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Attack Chooser/Player/UI Chooser")]
	public class UIBattleAttackChooser : BattleAttackChooser
	{
		public override UniTask<BattleAttack> ChooseAttack(BattleContext context, BattleUnit unit)
		{
			// await UniTask.CompletedTask;

			return BattleAttackChooserUIManager.I.ChooseAttack(context, unit);
			// TODO: Make and call the UI
			// throw new System.NotImplementedException();
		}

		public override UniTask<BattleAttack> RedoChoiceSelection(BattleContext context, BattleUnit unit, BattleAttack former)
		{
			return BattleAttackChooserUIManager.I.RedoChoiceSelection(context, unit, former);
			// return ChooseAttack(context, unit);
		}
	}
}