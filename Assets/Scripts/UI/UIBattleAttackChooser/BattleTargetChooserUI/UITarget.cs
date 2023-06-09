﻿using System;
using SaturnRPG.Battle;
using SaturnRPG.Camera3D2D;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.UI
{
	public class UITarget : MonoBehaviour
	{
		[SerializeField, Required]
		private CameraAnchorTransformation anchorTransformation;

		[SerializeField, Required, Header("Border")]
		private Image border;
		[SerializeField, Required]
		private RectTransform imageBorder;

		[SerializeField]
		private float sizeMultiplier = 1.25f;

		[SerializeField]
		private Color unusableInactive, usableInactive, unusableActive, usableActive;

		[ShowInInspector, ReadOnly]
		public ITargetable Targetable { get; private set; }
		[ShowInInspector, ReadOnly]
		public bool Usable { get; private set; }
		[ShowInInspector, ReadOnly]
		public bool Active { get; private set; }

		public event Action OnSelect;
		public event Action OnEnter;


		public void SetTarget(ITargetable targetable, bool usable)
		{
			Targetable = targetable;
			anchorTransformation.Set3DViewable(targetable.Viewable3D);

			// imageBorder.sizeDelta = targetable.Size.Size;
			imageBorder.SetSize(targetable.Size.Size * sizeMultiplier);
			SetUsable(usable);
		}

		private void SetUsable(bool usable)
		{
			Usable = usable;
			SetBorderColor(Active, Usable);
		}

		private void SetBorderColor(bool active, bool usable)
		{
			if (active)
				border.color = usable ? usableActive : unusableActive;
			else
				border.color = usable ? usableInactive : unusableInactive;
		}

		public void Select()
		{
			OnSelect?.Invoke();
		}

		public void Enter()
		{
			OnEnter?.Invoke();
		}

		public void SetActive(bool active)
		{
			Active = active;
			SetBorderColor(Active, Usable);
		}
	}
}