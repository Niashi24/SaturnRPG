using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Rendering.DistortedSprite
{
	public class DistortedSprite : MonoBehaviour
	{
		[field: SerializeField]
		[field: ValidateInput("CorrectSize", "Incorrect size.")]
		[field: OnValueChanged(nameof(UpdateMesh), true)]
		public Vector3[] Vertices { get; private set; }

		[SerializeField, Required]
		private MeshFilter meshFilter;

		[SerializeField, Required]
		private MeshRenderer meshRenderer;

		private Mesh mesh;
		
		private static readonly Vector2[] DEFAULT_UVS = new Vector2[] {
			Vector2.up,
			Vector2.one,
			Vector2.right,
			Vector2.zero
		};

		private static readonly int[] DEFAULT_TRIS = new int[] { 0, 1, 2, 0, 2, 3 };

		[Button]
		public void UpdateSprite(Sprite texture = null)
		{
			if (texture != null)
				meshRenderer.material.SetTexture("_MainTex", texture.texture);
		}

		[Button]
		public void UpdateMesh()
		{
			if (meshFilter == null) return;
			if (meshRenderer == null) return;

			if (Vertices is not { Length: 4 }) return;
			
			if (mesh == null) CreateMesh();

			mesh.vertices = Vertices;
			mesh.SetUVs(0, CalcUVQ());
			mesh.bounds = CalcBounds();
		}

		private Bounds CalcBounds()
		{
			// float minX = Mathf.Min(v1.x, v2.x, v3.x, v4.x);
			// float minY = Mathf.Min(v1.y, v2.y, v3.y, v4.y);
			//
			// float maxX = Mathf.Max(v1.x, v2.x, v3.x, v4.x);
			// float maxY = Mathf.Max(v1.y, v2.y, v3.y, v4.y);

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

		private void OnDrawGizmosSelected()
		{
			Vector3 p = transform.position;

			void DrawLine(Vector2 v1, Vector2 v2)
			{
				Debug.DrawLine((Vector3)v1 + p, (Vector3)v2 + p, Color.green);
			}

			for (int i = 0; i < Vertices.Length; i++)
			{
				DrawLine(Vertices[i], Vertices[(i + 1) % Vertices.Length]);
			}
			
			DrawLine(Vertices[0], Vertices[2]);
			DrawLine(Vertices[1], Vertices[3]);

			var intersection = (Vector3)GetIntersection();
			
			Gizmos.DrawWireCube(intersection + p, Vector2.one);
		}

		private Vector2 GetIntersection()
		{
			// v1 + v2 * t
			Vector2 v1 = Vertices[0];
			Vector2 v2 = Vertices[2] - Vertices[0];
			// v3 + v4 * t
			Vector2 v3 = Vertices[1];
			Vector2 v4 = Vertices[3] - Vertices[1];

			float determinant = v2.x * v4.y - v4.x * v2.y;
			if (determinant == 0) return Vector2.zero;
			
			// Derived from setting v1 + v2*t = v3 + v4*u
			float t = (v4.x * (v1.y - v3.y) + v4.y * (v3.x - v1.x)) / determinant;

			return v1 + v2 * t;
		}

		[Button]
		private Vector3[] CalcUVQ()
		{
			var intersectionPoint = GetIntersection();

			float DistanceToIntersectionPoint(Vector2 vertPos)
				=> Vector2.Distance(vertPos, intersectionPoint);

			float[] ds = new float[4];
			for (int i = 0; i < ds.Length; i++)
				ds[i] = DistanceToIntersectionPoint(Vertices[i]);

			Vector3 CalcUVQWithDist(int i, int j)
				=> new Vector3(DEFAULT_UVS[i].x, DEFAULT_UVS[i].y, 1) * ((ds[i] + ds[j]) / ds[j]);

			Vector3[] uvq = new Vector3[DEFAULT_UVS.Length];
			for (int i = 0; i < uvq.Length; i++)
				uvq[i] = CalcUVQWithDist(i, (i + 2) % 4);

			return uvq;
		}

		private void CreateMesh()
		{
			mesh = new Mesh()
			{
				vertices = new Vector3[4],
				uv = DEFAULT_UVS,
				triangles = DEFAULT_TRIS,
				name = "Distorted Sprite Mesh"
			};

			meshFilter.mesh = mesh;
		}
		
		#if UNITY_EDITOR
		private bool CorrectSize => Vertices.Length == 4;
		#endif
	}
}