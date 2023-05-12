using Cysharp.Threading.Tasks;
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

		[SerializeField, Required]
		private Text hpText, mpText;

		private BattleUnit _battleUnit;
		
		[SerializeField]
		GameObject[] toSetActive;

		private void OnDestroy()
		{
			if (_battleUnit == null) return;
			
			SubUnsubBattleUnit(false);
		}

		public void SetActive(bool active)
		{
			nameText.enabled = active;
			hpBar.Value?.SetActive(active);
			mpBar.Value?.SetActive(active);
			
			foreach (var obj in toSetActive)
				obj.SetActive(active);
		}

		public void SetUnit(BattleUnit unit)
		{
			if (_battleUnit != null)
				SubUnsubBattleUnit(false);

			_battleUnit = unit;
			SubUnsubBattleUnit(true);

			InitializeBars();
		}

		private void InitializeBars()
		{
			var stats = _battleUnit.GetBattleStats();
			
			hpText.text = $"{_battleUnit.HP}/{stats.HP}";
			mpText.text = $"{_battleUnit.MP}/{stats.MP}";

			if (stats.HP != 0)
				hpBar.Value.SetValueImmediate((float)_battleUnit.HP / stats.HP);
			if (stats.MP != 0)
				mpBar.Value.SetValueImmediate((float)_battleUnit.MP / stats.MP);
		}

		private void SubUnsubBattleUnit(bool subscribe)
		{
			_battleUnit.OnHPChange.SubUnsub(subscribe, HPChange);
			_battleUnit.OnMPChange.SubUnsub(subscribe, MPChange);
			UpdatePartyMember(_battleUnit.PartyMember);
		}

		private async UniTask HPChange(int newHP, int oldHP)
		{
		    if (newHP == oldHP) return;
		    
			var maxHP = _battleUnit.GetBattleStats().HP;

			hpText.text = $"{newHP}/{maxHP}";
			
			if (maxHP == 0) return;  // avoid / by 0
			var battleCancellationToken = BattleManager.I.BattleContext.BattleCancellationToken;
			
			await hpBar.Value.SetValueAsync((float)newHP / maxHP, battleCancellationToken);
		}

		private async UniTask MPChange(int newMP, int oldMP)
		{
		    if (newMP == oldMP) return;
		    
			var maxMP = _battleUnit.GetBattleStats().MP;

			mpText.text = $"{newMP}/{maxMP}";
			
			if (maxMP == 0) return;  // avoid / by 0
			var battleCancellationToken = BattleManager.I.BattleContext.BattleCancellationToken;
			
			await mpBar.Value.SetValueAsync((float)newMP / maxMP, battleCancellationToken);
		}

		private void UpdatePartyMember(PartyMember partyMember)
		{
			if (partyMember == null)
			{
				Debug.LogWarning("Party Member was null in Character Portrait", this);
				return;
			}
			nameText.text = partyMember.Name;
		}
	}
}