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
    }
}
