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
			=> BattleAttackChooserUIManager.I.ChooseAttack(context, unit);

		public override UniTask<BattleAttack> RedoChoiceSelection(BattleContext context, BattleUnit unit, BattleAttack former)
			=> BattleAttackChooserUIManager.I.RedoChoiceSelection(context, unit, former);
	}
}