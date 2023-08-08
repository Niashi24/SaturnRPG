using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SaturnRPG.Battle;

namespace SaturnRPG.UI.ScreenTransition
{
	public class ScreenTransition : MonoBehaviour
	{
		[SerializeField, Required]
		private Image screenCover;

		[SerializeField]
		private float transitionTime;

		private void OnEnable()
		{
			BattleLoadManager.OnStartLoadBattle.Subscribe(FadeInBattle);
			BattleLoadManager.OnStartLoadBattle.Subscribe(FadeOutBattle);
		}

		private void OnDisable()
		{
			BattleLoadManager.OnStartLoadBattle.Unsubscribe(FadeInBattle);
			BattleLoadManager.OnStartLoadBattle.Unsubscribe(FadeOutBattle);
		}

		private UniTask FadeInBattle(BattleEncounter battleEncounter)
		{
			return FadeInCover();
		}
		
		private UniTask FadeOutBattle(BattleEncounter battleEncounter)
		{
			return FadeOutCover();
		}

		private async UniTask FadeInCover()
		{
			await screenCover.DOFade(1, transitionTime).WithCancellation(this.GetCancellationTokenOnDestroy());
		}

		private async UniTask FadeOutCover()
		{
			await screenCover.DOFade(0, transitionTime).WithCancellation(this.GetCancellationTokenOnDestroy());
		}
	}
}