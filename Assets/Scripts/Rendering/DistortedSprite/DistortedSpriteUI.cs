using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Rendering.DistortedSprite
{
	public class DistortedSpriteUI : Graphic
	{
		[field: SerializeField]
		public Texture Texture { get; private set; }
		public override Texture mainTexture => Texture;
		
		[field: SerializeField]
		[field: ValidateInput("CorrectSize", "Incorrect size.")]
		public Vector3[] Vertices { get; private set; }
		
		private static readonly Vector2[] DEFAULT_UVS = new Vector2[] {
			Vector2.up,
			Vector2.one,
			Vector2.right,
			Vector2.zero
		};

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (Vertices is not { Length: 4 }) return;
			
			vh.Clear();

			UIVertex vert = UIVertex.simpleVert;
			var uvq = CalcUVQ();
			for (int i = 0; i < 4; i++)
			{
				vert.position = Vertices[i];
				vert.uv0 = uvq[i];
				vh.AddVert(vert);
			}
			
			vh.AddTriangle(0,1,2);
			vh.AddTriangle(0,2,3);
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
		
		#if UNITY_EDITOR
		private bool CorrectSize => Vertices.Length == 4;
		#endif
	}
}