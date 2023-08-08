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

		private void Update()
		{
			highlightImage.transform.localScale = (scaleCurve.Evaluate(Time.time) * Vector3.one).With(z: 1);
		}

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}