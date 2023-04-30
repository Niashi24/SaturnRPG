using System;
using SaturnRPG.Core;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Rendering.DistortedSprite
{
	public class DistortedSprite : MonoBehaviour, IDistortedSprite
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

		private static readonly int[] DEFAULT_TRIS = new int[] { 0, 1, 2, 0, 2, 3 };

		[Button]
		public void SetTexture(Texture texture = null)
		{
			if (texture != null)
				meshRenderer.material.SetTexture(Constants.MainTex, texture);
		}

		public void SetVertices(Vector3[] vertices)
		{
			if (vertices is not { Length : 4 }) return;
			Vertices = vertices;
			UpdateMesh();
		}

		[Button]
		private void UpdateMesh()
		{
			if (meshFilter == null) return;
			if (meshRenderer == null) return;

			if (Vertices is not { Length: 4 }) return;
			
			if (mesh == null) CreateMesh();

			mesh.vertices = Vertices;
			mesh.SetUVs(0, QuadraticProjection.CalcUVQ(Vertices));
			mesh.bounds = Vertices.CalcBounds();
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

			// var intersection = (Vector3)GetIntersection();
			//
			// Gizmos.DrawWireCube(intersection + p, Vector2.one);
		}

		private void CreateMesh()
		{
			mesh = new Mesh()
			{
				vertices = new Vector3[4],
				uv = QuadraticProjection.DEFAULT_UVS_2D,
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