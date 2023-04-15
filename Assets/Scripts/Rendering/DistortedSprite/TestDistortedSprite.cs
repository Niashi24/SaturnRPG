using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Rendering.DistortedSprite
{
	public class TestDistortedSprite : MonoBehaviour
	{
		[SerializeField]
		Vector2 v1,v2,v3,v4;

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
		public void UpdateMesh(Sprite texture = null)
		{
			if (meshFilter == null) return;
			if (meshRenderer == null) return;
			
			if (mesh == null) CreateMesh();
			
			if (texture != null)
				meshRenderer.material.SetTexture("_MainTex", texture.texture);

			mesh.vertices = new Vector3[] { v1, v2, v3, v4 };
			mesh.SetUVs(0, CalcUVQ());
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 p = transform.position;

			void DrawLine(Vector2 v1, Vector2 v2)
			{
				Debug.DrawLine((Vector3)v1 + p, (Vector3)v2 + p, Color.green);
			}
			
			DrawLine(v1, v2);
			DrawLine(v2, v3);
			DrawLine(v3, v4);
			DrawLine(v4, v1);
			
			DrawLine(v1, v3);
			DrawLine(v2, v4);

			var intersection = (Vector3)GetIntersection();
			
			Gizmos.DrawWireCube(intersection + p, Vector2.one);
		}

		private Vector2 GetIntersection()
		{
			// v1 + v2 * t
			Vector2 v1 = this.v1;
			Vector2 v2 = this.v3 - this.v1;
			// v3 + v4 * t
			Vector2 v3 = this.v2;
			Vector2 v4 = this.v4 - this.v2;

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
			ds[0] = DistanceToIntersectionPoint(v1);
			ds[1] = DistanceToIntersectionPoint(v2);
			ds[2] = DistanceToIntersectionPoint(v3);
			ds[3] = DistanceToIntersectionPoint(v4);

			Vector3 CalcUVQWithDist2(float d1, float d2, int vI)
				=> new Vector3(DEFAULT_UVS[vI].x, DEFAULT_UVS[vI].y, 1) * ((d1 + d2) / d2);

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
	}
}