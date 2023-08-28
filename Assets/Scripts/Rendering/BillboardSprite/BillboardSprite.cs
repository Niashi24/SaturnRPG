using System;
using System.Collections;
using System.Collections.Generic;
using LS.Utilities;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace SaturnRPG
{
	#if UNITY_EDITOR
	[ExecuteAlways]
	#endif
	public class BillboardSprite : MonoBehaviour
	{
		[SerializeField]
		private ValueReference<Camera> camera;

		[SerializeField]
		private ValueReference<Vector2> screenResolution = new(new(352, 240));

		[SerializeField]
		[Required]
		private SpriteRenderer spriteRenderer;
		
		#if UNITY_EDITOR
		[SerializeField]
		private bool runInEditorMode = true;
		#endif

		private void LateUpdate()
		{
			#if UNITY_EDITOR
			if (!runInEditorMode) return;
			#endif
			Billboard();
			Scale();
		}

		private void Billboard()
		{
			spriteRenderer.transform.rotation = camera.Value.transform.rotation;
		}

		private void Scale()
		{
			var camera = this.camera.Value;
			var screenHeight = screenResolution.Value.y;
			// https://gamedev.stackexchange.com/questions/204167/resize-sprite-to-match-screen-size
			// Angle the camera can see above the center.
			float halfFovRadians = camera.fieldOfView * Mathf.Deg2Rad / 2f;

			// What depth is the object at in terms of the camera?
			float depth = Vector3.Dot(spriteRenderer.transform.position - camera.transform.position,
				camera.transform.forward);

			// How high is it from top to bottom of the view frustum,
			// in world space units, at our target depth?
			float visibleHeightAtDepth = depth * Mathf.Tan(halfFovRadians) * 2f;

			// How many times bigger (or smaller) is the height we want to fill?
			float scaleFactor = visibleHeightAtDepth / screenHeight;

			// Scale to fit, uniformly on all axes.
			spriteRenderer.transform.localScale = Vector3.one * scaleFactor;
		}
	}
}