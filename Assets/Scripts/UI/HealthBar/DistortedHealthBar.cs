using SaturnRPG.Rendering.DistortedSprite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
    public class DistortedHealthBar : MonoBehaviour, ValueBarDisplay
    {
        [SerializeField, Required]
        private DistortedSpriteUI left, middle, right;

        private Vector3[] vertLeft, vertMiddle, vertRight;

        [SerializeField]
        [OnValueChanged("Test")]
        private float cornerOffset = 0.1f;

        [SerializeField]
        [OnValueChanged("Test", true)]
        private RectTransform rectTransform;

        private void Test()
        {
            SetValues(0.33f, 0.66f);
        }

        [Button]
        public void SetValues(float actual, float target)
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            var rect = rectTransform.rect;
            
            // Debug.Log(rect);

            if (vertLeft is not { Length: 4 } ) vertLeft = new Vector3[4];
            if (vertMiddle is not { Length: 4 } ) vertMiddle = new Vector3[4];
            if (vertRight is not { Length: 4 } ) vertRight = new Vector3[4];
        
            //   0 - 1 - 2 - 3
            //  /   /   /   /
            // 4 - 5 - 6 - 7

            var v0 = new Vector3(rect.xMin, rect.yMax);
            var v3 = new Vector3(rect.xMax, rect.yMax);
            v0 = Vector3.LerpUnclamped(v0, v3, cornerOffset);
            var v4 = new Vector3(rect.xMin, rect.yMin);
            var v7 = new Vector3(rect.xMax, rect.yMin);
            v7 = Vector3.LerpUnclamped(v7, v4, cornerOffset);

            float low = actual, high = target;
            if (low > high)
                (low, high) = (high, low);

            var v1 = Vector3.Lerp(v0, v3, low);
            var v2 = Vector3.Lerp(v0, v3, high);
            var v5 = Vector3.Lerp(v4, v7, low);
            var v6 = Vector3.Lerp(v4, v7, high);

            vertLeft[0] = v0;
            vertLeft[1] = v1;
            vertLeft[2] = v5;
            vertLeft[3] = v4;

            vertMiddle[0] = v1;
            vertMiddle[1] = v2;
            vertMiddle[2] = v6;
            vertMiddle[3] = v5;

            vertRight[0] = v2;
            vertRight[1] = v3;
            vertRight[2] = v7;
            vertRight[3] = v6;

            
            left.SetVertices(vertLeft);
            middle.SetVertices(vertMiddle);
            right.SetVertices(vertRight);
        }
    }
}