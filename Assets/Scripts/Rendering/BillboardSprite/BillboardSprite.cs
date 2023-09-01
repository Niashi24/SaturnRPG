using System;
using System.Collections;
using System.Collections.Generic;
using LS.Utilities;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG
{
	#if UNITY_EDITOR
	[ExecuteAlways]
	#endif
	public class BillboardSprite : MonoBehaviour
	{
		[SerializeField]
		[Required]
		private SpriteRenderer spriteRenderer;

		private void OnEnable()
		{
			BillboardSpriteManager.Sprites.Add(spriteRenderer.transform);
		}

		private void OnDisable()
		{
			BillboardSpriteManager.Sprites.Remove(spriteRenderer.transform);
		}
	}
}