using UnityEngine;

namespace SaturnRPG.Rendering.DistortedSprite
{
    public interface IDistortedSprite
    {
        Vector3[] Vertices { get; }

        void SetTexture(Texture texture);
        void SetVertices(Vector3[] vertices);
    }
}