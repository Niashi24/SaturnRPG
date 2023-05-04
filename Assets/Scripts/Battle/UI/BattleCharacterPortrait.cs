using System;
using LS.Utilities;
using SaturnRPG.UI.HealthBar;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Battle
{
	public class BattleCharacterPortrait : MonoBehaviour
	{
		[SerializeField, Required]
		private Text nameText;

		[SerializeField, Required]
		private ObjectReference<IValueBar> hpBar, mpBar;

		private BattleUnit _battleUnit;

		private void OnDisable()
		{
			if (_battleUnit == null) return;
			
			
		}

		public void SetActive(bool active)
		{
			nameText.enabled = active;
			hpBar.Value?.SetActive(active);
			mpBar.Value?.SetActive(active);
		}

		public void SetUnit(BattleUnit unit)
		{
			_battleUnit = unit;
		}
	}
}