using System.Collections.Generic;
using LS.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG
{
	public class BillboardSpriteManager : MonoBehaviour
	{
		public static readonly List<Transform> Sprites = new();

		[SerializeField]
		private ValueReference<Camera> cameraReference;

		[SerializeField]
		[ValidateInput(nameof(ScreenYMustNotBeZero), "Screen height must not be zero.")]
		private ValueReference<Vector2> screenResolution = new(new(352, 240));

		private void LateUpdate()
		{
			var camera = cameraReference.Value;
			var cameraTransform = camera.transform;
			var forward = cameraTransform.forward;
			var screenHeight = this.screenResolution.Value.y;
			float tanHalfFovX2 = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2f) * 2f;

			foreach (var spriteTransform in Sprites)
			{
				// face sprite towards camera
				spriteTransform.rotation = cameraTransform.rotation;
				
				// https://gamedev.stackexchange.com/questions/204167/resize-sprite-to-match-screen-size

				// What depth is the object at in terms of the camera?
				float depth = Vector3.Dot(spriteTransform.position - cameraTransform.position,
					forward);

				// How high is it from top to bottom of the view frustum,
				// in world space units, at our target depth?
				float visibleHeightAtDepth = depth * tanHalfFovX2;

				// How many times bigger (or smaller) is the height we want to fill?
				float scaleFactor = visibleHeightAtDepth / screenHeight;

				// Scale to fit, uniformly on all axes.
				spriteTransform.transform.localScale = Vector3.one * scaleFactor;
			}
		}

		private bool ScreenYMustNotBeZero() => screenResolution.Value.y != 0;
	}
}