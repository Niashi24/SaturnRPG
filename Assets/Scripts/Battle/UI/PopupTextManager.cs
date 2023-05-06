using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace SaturnRPG.Battle.UI
{
	public class PopupTextManager : MonoSingleton<PopupTextManager>
	{
		[SerializeField, Required]
		private PopupText textPrefab;

		private ObjectPool<PopupText> _textPool;

		private void Start()
		{
			_textPool = textPrefab.CreateMonoPool();
		}

		public PopupText CreatePopupText(PopupTextParams popupTextParams, Transform parent, Vector3 offset)
		{
			var popupText = _textPool.Get();
			
			Transform textTransform = popupText.transform;
			textTransform.SetParent(parent);
			textTransform.localPosition = offset;
			
			popupText.SetParams(popupTextParams);
			popupText.SetManager(this);

			return popupText;
		}

		public UniTask AwaitPopupText(PopupTextParams popupTextParams, Transform parent, Vector3 offset)
		{
			var popupText = CreatePopupText(popupTextParams, parent, offset);

			return popupText.PopupCoroutine();
		}

		public void Release(PopupText popupText)
		{
			popupText.transform.SetParent(transform);
			_textPool.Release(popupText);
		}
	}
}