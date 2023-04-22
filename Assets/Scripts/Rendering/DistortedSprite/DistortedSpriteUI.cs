using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Rendering.DistortedSprite
{
	[RequireComponent(typeof(CanvasRenderer))]
	public class DistortedSpriteUI : Graphic, IDistortedSprite
	{
		[field: SerializeField]
		public Texture Texture { get; private set; }
		public override Texture mainTexture => Texture;
		
		[field: SerializeField]
		[field: ValidateInput("CorrectSize", "Incorrect size.")]
		public Vector3[] Vertices { get; private set; }

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (Vertices is not { Length: 4 }) return;
			
			vh.Clear();

			UIVertex vert = UIVertex.simpleVert;
			var uvq = QuadraticProjection.CalcUVQ(Vertices);
			for (int i = 0; i < 4; i++)
			{
				vert.position = Vertices[i];
				vert.uv0 = uvq[i];
				vh.AddVert(vert);
			}
			
			vh.AddTriangle(0,1,2);
			vh.AddTriangle(0,2,3);
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
		}

		[Button]
		public void SetVertices(Vector3[] vertices)
		{
			if (vertices is not { Length: 4 } ) return;
			Vertices = vertices;
			SetVerticesDirty();
		}

		public void SetTexture(Texture texture)
		{
			if (texture == null || texture == Texture) return;
			Texture = texture;
			SetMaterialDirty();
		}
		
		#if UNITY_EDITOR
		private bool CorrectSize => Vertices.Length == 4;
		#endif
	}
}