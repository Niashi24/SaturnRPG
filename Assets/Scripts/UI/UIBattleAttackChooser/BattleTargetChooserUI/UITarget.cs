using System;
using SaturnRPG.Battle;
using SaturnRPG.Camera3D2D;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.UI
{
	public class UITarget : MonoBehaviour
	{
		[SerializeField, Required]
		private CameraAnchorTransformation anchorTransformation;

		[SerializeField, Required]
		private Image border;

		[SerializeField]
		private Color usableColor, unusableColor;

		public ITargetable Targetable { get; private set; }
		public bool Usable { get; private set; }

		public event Action OnSelect;
		public event Action OnEnter;

		public void SetTarget(ITargetable targetable, bool usable)
		{
			Targetable = targetable;
			anchorTransformation.Set3DViewable(targetable.Viewable3D);
			SetUsable(usable);
		}

		private void SetUsable(bool usable)
		{
			Usable = usable;
			border.color = usable ? usableColor : unusableColor;
		}

		public void Select()
		{
			OnSelect?.Invoke();
		}

		public void Enter()
		{
			OnEnter?.Invoke();
		}

		private void LateUpdate()
		{
			
		}
	}
}