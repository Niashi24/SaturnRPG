using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.UI
{
	public class BattleCharacterPortraitManager : MonoBehaviour
	{
		[SerializeField]
		private BattleCharacterPortrait[] characterPortraits;

		[SerializeField, Required]
		private BattleUnitManager battleUnitManager;

		private void OnEnable()
		{
			battleUnitManager.OnSetActiveUnits += UpdateCharacterPortraits;
		}

		private void OnDisable()
		{
			battleUnitManager.OnSetActiveUnits -= UpdateCharacterPortraits;
		}

		private void UpdateCharacterPortraits(List<BattleUnit> battleUnits)
		{
			foreach (var portrait in characterPortraits)
				portrait.SetActive(false);

			for (int i = 0; i < battleUnits.Count && i < characterPortraits.Length; i++)
			{
				characterPortraits[i].SetUnit(battleUnits[i]);
				characterPortraits[i].SetActive(true);
			}
			
			if (battleUnits.Count > characterPortraits.Length)
				Debug.LogWarning("Not enough character portraits for the amount of characters.");
		}
	}
}