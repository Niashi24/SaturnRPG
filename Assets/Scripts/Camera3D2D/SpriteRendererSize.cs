using UnityEngine;

namespace SaturnRPG.Camera3D2D
{
    [System.Serializable]
    public class SpriteRendererSize : ISize
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public SpriteRendererSize() {}
    
        public SpriteRendererSize(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }

        public Vector2 Size => spriteRenderer != null ? spriteRenderer.bounds.size : Vector2.one;
    }
}