using SaturnRPG.Utilities.Extensions;
using UnityEngine;

namespace SaturnRPG.Rendering.DistortedSprite
{
    public static class QuadraticProjection
    {
        public static readonly Vector2[] DEFAULT_UVS_2D = {
            Vector2.up,
            Vector2.one,
            Vector2.right,
            Vector2.zero
        };

        public static readonly Vector3[] DEFAULT_UVS_3D = {
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        };
        
        public static Vector3[] CalcUVQ(Vector3[] vertices)
        {
            if (vertices is not { Length : 4 })
                return DEFAULT_UVS_3D;
            
            var intersectionPoint = VectorExtensions.GetIntersectionPoint(
                vertices[0],
                vertices[2] - vertices[0],
                vertices[1],
                vertices[3] - vertices[1]);

            float DistanceToIntersectionPoint(Vector2 vertPos)
                => Vector2.Distance(vertPos, intersectionPoint);

            float[] ds = new float[4];
            for (int i = 0; i < ds.Length; i++)
                ds[i] = DistanceToIntersectionPoint(vertices[i]);

            Vector3 CalcUVQWithDist(int i, int j)
                => new Vector3(DEFAULT_UVS_3D[i].x, DEFAULT_UVS_3D[i].y, 1) * ((ds[i] + ds[j]) / ds[j]);

            Vector3[] uvq = new Vector3[DEFAULT_UVS_3D.Length];
            for (int i = 0; i < uvq.Length; i++)
                uvq[i] = CalcUVQWithDist(i, (i + 2) % 4);

            return uvq;
        }
    }
}