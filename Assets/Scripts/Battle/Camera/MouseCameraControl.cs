using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LS.Utilities;
using SaturnRPG.Battle;
using SaturnRPG.UI;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG
{
    public class MouseCameraControl : MonoBehaviour, I3DViewable
    {
        [SerializeField]
        private ValueReference<Camera> cameraReference;

        [SerializeField]
        private AnimationCurve mouseXCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private float xPadding;

        [SerializeField]
        private float cameraWidth = 60f;

        [ShowInInspector, ReadOnly]
        private Vector2 _minMaxX;
        [ShowInInspector, ReadOnly]
        private Vector3 _position = Vector3.zero;

        private void Update()
        {
            float mousePos01 = getMouseX01();

            _position = new Vector3(Mathf.Lerp(_minMaxX.x, _minMaxX.y, mouseXCurve.Evaluate(mousePos01)), 0, 0);
        }

        private float getMouseX01()
        {
            if (cameraReference.Value == null) return 0.5f;
            return cameraReference.Value.ScreenToViewportPoint(Input.mousePosition).x;
        }

        [Button]
        public void SetMinMaxX(Vector2 bounds)
        {
            _minMaxX = bounds;
        }

        public void LoadTargets(List<UITarget> targets)
        {
            var (min, max) = targets.Select(x => x.Targetable.Viewable3D.GetPosition().x).MinMax();

            var middle = ((min + max) / 2);
            
            var distanceToEnd = Mathf.Max(0, max + xPadding - middle - cameraWidth);

            var bounds = new Vector2(middle - distanceToEnd, middle + distanceToEnd);
            
            SetMinMaxX(bounds);
        }

        public Vector3 GetPosition() => _position;
    }
}
