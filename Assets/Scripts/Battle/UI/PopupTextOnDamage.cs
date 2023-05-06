using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.UI
{
	public class PopupTextOnDamage : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleUnitManager playerUnitManager, enemyUnitManager;

		[SerializeField]
		private float animationTimeSeconds = 1.5f;

		[SerializeField]
		private Vector3 popupOffset = Vector3.up * 4;

		private void OnEnable()
		{
			playerUnitManager.OnSetActiveUnits += SubscribeHitAnimation;
			enemyUnitManager.OnSetActiveUnits += SubscribeHitAnimation;
		}

		private void OnDisable()
		{
			playerUnitManager.OnSetActiveUnits -= SubscribeHitAnimation;
			enemyUnitManager.OnSetActiveUnits -= SubscribeHitAnimation;
		}

		private void SubscribeHitAnimation(List<BattleUnit> battleUnits)
		{
			foreach (var battleUnit in battleUnits)
			{
				var bunit = battleUnit;  // copy variable to use in closure
				
				battleUnit.OnHPChange.Subscribe(HitAnim);

				UniTask HitAnim(int newHP, int oldHP) => PlayHitAnimationOnHit(bunit, newHP, oldHP);
			}
		}

		private UniTask PlayHitAnimationOnHit(BattleUnit unit, int newHP, int oldHP)
		{
			// TODO: Change color of text based on hit vs restore health
			var popupParams = new PopupTextParams()
			{
				Message = $"{Math.Abs(oldHP - newHP)}",
				AnimationTimeSeconds = (int?)animationTimeSeconds
			};
			
			return PopupTextManager.I.AwaitPopupText(popupParams, unit.UnitVisual.transform, popupOffset);
		}
	}
}