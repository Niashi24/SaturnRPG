using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.UI
{
	public class UITab : MonoBehaviour
	{
		[SerializeField, Required]
		private Image image;

		[SerializeField, Required]
		private Sprite inactive, highlight, active;

		public event Action OnSelect;
		
		public bool Active { get; private set; }

		public void Enter()
		{
			if (Active) return;
			
			image.sprite = highlight;
		}

		public void Exit()
		{
			if (Active) return;
			
			image.sprite = inactive;
		}

		public void SetActive(bool active)
		{
			Active = active;
			image.sprite = active ? this.active : inactive;
		}

		public void Select()
		{
			SetActive(true);
			OnSelect?.Invoke();
		}

		public void Deselect()
		{
			SetActive(false);
		}
	}
}