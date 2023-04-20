using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Rendering.DistortedSprite
{
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

		[Button]
		public void SetVertices(Vector3[] vertices)
		{
			if (vertices is not { Length: 4 } ) return;
			Vertices = vertices;
			SetVerticesDirty();
		}

		public void SetTexture(Texture texture)
		{
			if (texture == null) return;
			Texture = texture;
			SetMaterialDirty();
		}
		
		#if UNITY_EDITOR
		private bool CorrectSize => Vertices.Length == 4;
		#endif
	}
}