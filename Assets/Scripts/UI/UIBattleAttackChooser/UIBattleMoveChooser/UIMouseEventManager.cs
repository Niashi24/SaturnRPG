using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SaturnRPG.UI
{
	public class UIMouseEventManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[field: SerializeField]
		public UnityEvent OnEnter { get; private set; }
		[field: SerializeField]
		public UnityEvent OnExit { get; private set; }
		[field: SerializeField]
		public UnityEvent OnClick { get; private set; }
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			OnEnter.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			OnExit.Invoke();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClick.Invoke();
		}
	}
}