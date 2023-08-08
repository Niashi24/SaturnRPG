using System.Collections.Generic;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class BattleTextTargetName : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleTargetChooserUI battleTargetChooserUI;

		[SerializeField, Required]
		private BattleText battleText;

		private void Awake()
		{
			battleTargetChooserUI.OnStartSelection += EnableTextAndFill;
			battleTargetChooserUI.OnChangeSelection += TargetChanged;
			
		}

		private void OnDestroy()
		{
			
		}

		private void EnableTextAndFill(List<UITarget> uiTargets)
		{
			battleText.SetTextAndActive("");
		}

		private void TargetChanged(UITarget uiTarget)
		{
			battleText.SetText(uiTarget == null ? "" : uiTarget.Targetable?.Name ?? "");
		}

		private void DisableText()
		{
			battleText.SetActive(false);
		}
	}
}