using System.Threading;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Battle.UI
{
	public class PopupText : MonoBehaviour
	{
		[SerializeField, Required]
		private Text text;

		[SerializeField]
		private AnimationCurve yAnimationCurve;
		
		[SerializeField]
		private float animationTimeSeconds = 2;

		private PopupTextManager _manager;
		private CancellationTokenSource _cancellationTokenSource;

		public void SetManager(PopupTextManager manager)
		{
			_manager = manager;
		}

		public void SetParams(PopupTextParams popupTextParams)
		{
			text.text = popupTextParams.Message;
			animationTimeSeconds = popupTextParams.AnimationTimeSeconds ?? animationTimeSeconds;
		}

		public void Cancel()
		{
			_cancellationTokenSource.Cancel();
		}

		public async UniTask PopupCoroutine()
		{
			if (animationTimeSeconds == 0) return;
			
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource =
				CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

			var cancelToken = _cancellationTokenSource.Token;

			var popupTransform = this.transform;

			var initialPosition = popupTransform.localPosition;

			float accumulatedTime = 0;
			while (accumulatedTime < animationTimeSeconds)
			{
				var t = accumulatedTime / animationTimeSeconds;
				popupTransform.localPosition = initialPosition + Vector3.up * yAnimationCurve.Evaluate(t);
				// Snap to Grid
				popupTransform.position = popupTransform.position.Round();

				await UniTask.Yield(cancelToken);
				accumulatedTime = Mathf.Min(accumulatedTime + Time.deltaTime, animationTimeSeconds);
			}
			
			_manager.Release(this);
		}
	}
}