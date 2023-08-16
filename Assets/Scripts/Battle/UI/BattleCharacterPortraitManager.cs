using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.UI;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.UI
{
	public class BattleCharacterPortraitManager : MonoBehaviour
	{
		[SerializeField]
		private BattleCharacterPortrait[] characterPortraits;

		[SerializeField, Required]
		private BattleAttackChooserUIManager battleAttackChooser;

		[SerializeField, Required]
		private BattleUnitManager battleUnitManager;

		private void OnEnable()
		{
			battleUnitManager.OnSetActiveUnits += UpdateCharacterPortraits;
			battleAttackChooser.OnStartChooseAttack.Subscribe(HighlightWhenActiveSelection);
			battleAttackChooser.OnChooseAttack.Subscribe(DisableWhenDone);
			battleAttackChooser.OnCancelChooseAttack.Subscribe(DisableWhenCancelled);
		}

		private void OnDisable()
		{
			battleUnitManager.OnSetActiveUnits -= UpdateCharacterPortraits;
			battleAttackChooser.OnStartChooseAttack.Unsubscribe(HighlightWhenActiveSelection);
			battleAttackChooser.OnChooseAttack.Unsubscribe(DisableWhenDone);
			battleAttackChooser.OnCancelChooseAttack.Unsubscribe(DisableWhenCancelled);
		}

		private void UpdateCharacterPortraits(List<BattleUnit> battleUnits)
		{
			foreach (var portrait in characterPortraits)
				portrait.SetActive(false);

			for (int i = 0; i < battleUnits.Count && i < characterPortraits.Length; i++)
			{
				characterPortraits[i].SetUnit(battleUnits[i]);
				characterPortraits[i].SetActive(true);
				RemoveWorldHealthBar(battleUnits[i]);
			}

			if (battleUnits.Count > characterPortraits.Length)
				Debug.LogWarning("Not enough character portraits for the amount of characters.");
		}

		private void RemoveWorldHealthBar(BattleUnit unit)
		{
			unit.UnitVisual.HealthBar.Value.SetActive(false);
		}

		private UniTask HighlightWhenActiveSelection(BattleUnit unit, BattleContext _)
		{
			FindAndSetActiveHighlight(unit, true);
			return UniTask.CompletedTask;
		}

		private UniTask DisableWhenDone(BattleAttack attack)
		{
			FindAndSetActiveHighlight(attack.User, false);
			return UniTask.CompletedTask;
		}

		private UniTask DisableWhenCancelled(BattleUnit unit, BattleContext _)
		{
			FindAndSetActiveHighlight(unit, false);
			return UniTask.CompletedTask;
		}

		private void FindAndSetActiveHighlight(BattleUnit unit, bool active)
		{
			var portrait = characterPortraits.FirstWhere(x => x.BattleUnit == unit);
			if (portrait != null)
				portrait.PortraitHighlight.SetActive(active);
		}
	}
}