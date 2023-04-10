using System;
using System.Collections.Generic;
using LS.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Camera3D2D
{
    [ExecuteAlways]
    public class CameraAnchorTransformation : MonoBehaviour
    {
        [Header("Cameras")]

        [SerializeField]
        private ValueReference<Camera> inputCamera;

        [SerializeField]
        private ValueReference<Camera> outputCamera;
        
        [field: Header("Anchor")]

        [field: SerializeField]
        public Transform Anchor { get; private set; }

        //The relative position on the current the Anchor will set the position to
        [SerializeField]
        [Range(-1, 1)]
        private float anchorX = 0;
        [SerializeField]
        [Range(-1, 1)]
        private float anchorY = 0;

        [SerializeReference]
        [Tooltip("The size used when calculating the position of the Anchor")]
        private ISize sizeReference = new ManualSize();
        
        [Header("Camera Position Types")]

        [SerializeField]
        [OnValueChanged(nameof(UpdateCameraFunctions))]
        [Tooltip("Anchor position is transferred from Input Camera using Input type into Middle Type")]
        private CameraPositionType inputCameraType;
        
        [SerializeField]
        [OnValueChanged(nameof(UpdateCameraFunctions))]
        private CameraPositionType middleCameraType = CameraPositionType.VIEWPORT;
        
        [SerializeField]
        [OnValueChanged(nameof(UpdateCameraFunctions))]
        [Tooltip("Translated position is transferred from Middle Type into output position using Output Camera")]
        private CameraPositionType outputCameraType;

        private Func<Vector3, Camera, Vector3> _inputFunction = defaultIdentityFunction;
        private Func<Vector3, Camera, Vector3> _outputFunction = defaultIdentityFunction;

        private void OnEnable()
        {
            UpdateCameraFunctions();
        }

        void LateUpdate()
        {
            if (inputCamera.Value == null) return;
            if (outputCamera.Value == null) return;
            if (Anchor == null) return;
            if (sizeReference == null) sizeReference = new ManualSize();

            Vector2 size = sizeReference.Size;
            Vector2 halfSize = size / 2;

            Vector3 position = _outputFunction(_inputFunction(Anchor.position, inputCamera.Value), outputCamera.Value);
            position += new Vector3(-anchorX * halfSize.x, -anchorY * halfSize.y);

            transform.position = position;
        }

        public void SetAnchor(Transform anchor)
        {
            if (anchor == null) return;
            this.Anchor = anchor;
        }

        public void SetInputCamera(Camera inputCamera)
        {
            if (inputCamera == null) return;
            this.inputCamera.Value = inputCamera;
        }

        public void SetOutputCamera(Camera outputCamera)
        {
            if (outputCamera == null) return;
            this.outputCamera.Value = outputCamera;
        }

        private void UpdateCameraFunctions()
        {
            _inputFunction = CameraTransformationToFunction(inputCameraType, middleCameraType);
            _outputFunction = CameraTransformationToFunction(middleCameraType, outputCameraType);
        }

        [ContextMenu("Use Manual Size")]
        private void SetToManual()
        {
            sizeReference = new ManualSize();
        }

        [ContextMenu("Use Sprite Renderer Size")]
        private void SetToSpriteRenderer()
        {
            sizeReference = new SpriteRendererSize(GetComponent<SpriteRenderer>());
        }

        public enum CameraPositionType {WORLD, SCREEN_POINT, VIEWPORT};

        private static readonly Dictionary<(CameraPositionType, CameraPositionType), Func<Vector3, Camera, Vector3>> inputOutputTypeToFunction = new()
        {
            {(CameraPositionType.WORLD, CameraPositionType.SCREEN_POINT), WorldToScreenPoint},
            {(CameraPositionType.WORLD, CameraPositionType.VIEWPORT), WorldToViewport},
            {(CameraPositionType.SCREEN_POINT, CameraPositionType.WORLD), ScreenPointToWorld},
            {(CameraPositionType.SCREEN_POINT, CameraPositionType.VIEWPORT), ScreenPointToViewport},
            {(CameraPositionType.VIEWPORT, CameraPositionType.WORLD), ViewportToWorldPoint},
            {(CameraPositionType.VIEWPORT, CameraPositionType.SCREEN_POINT), ViewportToScreenPoint}
        };

        private static readonly Func<Vector3, Camera, Vector3> defaultIdentityFunction = (x,y) => x;
        Func<Vector3, Camera, Vector3> CameraTransformationToFunction(CameraPositionType input, CameraPositionType output)
        {
            if (inputOutputTypeToFunction.ContainsKey((input, output))) return inputOutputTypeToFunction[(input, output)];
            return defaultIdentityFunction;
        }

        private static Vector3 WorldToScreenPoint(Vector3 pos, Camera camera) => camera.WorldToScreenPoint(pos);
        private static Vector3 WorldToViewport(Vector3 pos, Camera camera) => camera.WorldToViewportPoint(pos);
        private static Vector3 ScreenPointToWorld(Vector3 pos, Camera camera) => camera.ScreenToWorldPoint(pos);
        private static Vector3 ScreenPointToViewport(Vector3 pos, Camera camera) => camera.ScreenToViewportPoint(pos);
        private static Vector3 ViewportToWorldPoint(Vector3 pos, Camera camera) => camera.ViewportToWorldPoint(pos);
        private static Vector3 ViewportToScreenPoint(Vector3 pos, Camera camera) => camera.ViewportToScreenPoint(pos);
    }
}