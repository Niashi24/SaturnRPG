using Cysharp.Threading.Tasks;
using LS.Utilities;
using SaturnRPG.Battle.UI;
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

		[field: SerializeField, Required]
		public CharacterPortraitHighlight PortraitHighlight { get; private set; }

		public BattleUnit BattleUnit { get; private set; }
		
		[SerializeField]
		GameObject[] toSetActive;

		private void OnDestroy()
		{
			if (BattleUnit == null) return;
			
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
			if (BattleUnit != null)
				SubUnsubBattleUnit(false);

			BattleUnit = unit;
			SubUnsubBattleUnit(true);

			InitializeBars();
		}

		private void InitializeBars()
		{
			var stats = BattleUnit.GetBattleStats();
			
			hpText.text = $"{BattleUnit.HP}/{stats.HP}";
			mpText.text = $"{BattleUnit.MP}/{stats.MP}";

			if (stats.HP != 0)
				hpBar.Value.SetValueImmediate((float)BattleUnit.HP / stats.HP);
			if (stats.MP != 0)
				mpBar.Value.SetValueImmediate((float)BattleUnit.MP / stats.MP);
		}

		private void SubUnsubBattleUnit(bool subscribe)
		{
			BattleUnit.OnHPChange.SubUnsub(subscribe, HPChange);
			BattleUnit.OnMPChange.SubUnsub(subscribe, MPChange);
			UpdatePartyMember(BattleUnit.PartyMember);
		}

		private async UniTask HPChange(int newHP, int oldHP)
		{
		    if (newHP == oldHP) return;
		    
			var maxHP = BattleUnit.GetBattleStats().HP;

			hpText.text = $"{newHP}/{maxHP}";
			
			if (maxHP == 0) return;  // avoid / by 0
			var battleCancellationToken = BattleManager.I.BattleContext.BattleCancellationToken;
			
			await hpBar.Value.SetValueAsync((float)newHP / maxHP, battleCancellationToken);
		}

		private async UniTask MPChange(int newMP, int oldMP)
		{
		    if (newMP == oldMP) return;
		    
			var maxMP = BattleUnit.GetBattleStats().MP;

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