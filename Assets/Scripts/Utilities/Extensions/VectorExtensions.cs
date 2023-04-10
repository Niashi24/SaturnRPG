using UnityEngine;

namespace SaturnRPG.Utilities.Extensions
{
	public static class VectorExtensions
	{
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(
				x ?? original.x,
				y ?? original.y,
				z ?? original.z
			);
		}

		public static Vector3 WithMagnitude(this Vector3 original, float magnitude)
			=> original.normalized * magnitude;
	}
}