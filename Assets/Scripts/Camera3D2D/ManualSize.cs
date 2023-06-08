using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.Camera3D2D
{
    [System.Serializable]
    public class ManualSize : ISize
    {
        [SerializeField]
        private Vector2 size;

        public Vector2 Size => size;

        public ManualSize()
        {
            size = Vector2.zero;
        }

        public ManualSize(Vector2 size)
        {
            this.size = size;
        }
    }
}
