﻿using UnityEngine;

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
		
		public static Bounds CalcBounds(this Vector3[] Vertices)
		{

			float minX, minY, maxX, maxY;

			minX = maxX = Vertices[0].x;
			minY = maxY = Vertices[0].y;

			for (int i = 1; i < Vertices.Length; i++)
			{
				if (Vertices[i].x < minX)
					minX = Vertices[i].x;
				if (Vertices[i].y < minY)
					minY = Vertices[i].y;
				if (Vertices[i].x > maxX)
					maxX = Vertices[i].x;
				if (Vertices[i].y > maxY)
					maxY = Vertices[i].y;
			}

			return new Bounds()
			{
				min = new Vector3(minX, minY, 0),
				max = new Vector3(maxX, maxY, 0)
			};
		}

		// Returns the intersection point of the vector lines
		// v = v1 + v2 * t and w = v3 + v4 * u
		public static Vector2 GetIntersectionPoint(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
		{
			float determinant = v2.x * v4.y - v4.x * v2.y;
			if (determinant == 0) return Vector2.zero;
			
			// Derived from setting v1 + v2*t = v3 + v4*u
			float t = (v4.x * (v1.y - v3.y) + v4.y * (v3.x - v1.x)) / determinant;

			return v1 + v2 * t;
		}
	}
}