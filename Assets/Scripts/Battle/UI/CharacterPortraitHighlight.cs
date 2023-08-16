using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Battle.UI
{
	public class CharacterPortraitHighlight : MonoBehaviour
	{
		[SerializeField, Required]
		private Image highlightImage;

		[SerializeField]
		private AnimationCurve scaleCurve;

		private Vector2 _baseSize;

		private void Start()
		{
			_baseSize = highlightImage.rectTransform.rect.size;
		}

		private void LateUpdate()
		{
			highlightImage.rectTransform.SetSize((scaleCurve.Evaluate(Time.time) * _baseSize).RoundTo(2));
		}

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}