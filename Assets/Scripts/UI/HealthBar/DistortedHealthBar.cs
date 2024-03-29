﻿using LS.Utilities;
using SaturnRPG.Rendering.DistortedSprite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
    public class DistortedHealthBar : MonoBehaviour, ValueBarDisplay
    {
        [SerializeField, Required]
        private ObjectReference<IDistortedSprite> left, middle, right;

        private Vector3[] vertLeft, vertMiddle, vertRight;

        [ShowInInspector, OnValueChanged("UpdateDisplay")]
        public float Value { get; private set; } = 0.5f;  // set to 0.5 for testing

        [ShowInInspector, OnValueChanged("UpdateDisplay")]
        public float Target { get; private set; } = 1;

        [SerializeField, Required]
        private Texture healthTexture, healTexture, damageTexture, emptyTexture;

        [SerializeField]
        [OnValueChanged("UpdateDisplay")]
        private float cornerOffset = 0.1f;

        [SerializeField]
        [OnValueChanged("UpdateDisplay")]
        private RectTransform rectTransform;

        private void Start()
        {
            UpdateTextures();
        }

        [Button]
        private void UpdateDisplay()
        {
            SetValues(Value, Target);
        }

        [Button]
        private void UpdateTextures()
        {
            if (left.HasValue)
                left.Value.SetTexture(healthTexture);
            if (right.HasValue)
                right.Value?.SetTexture(emptyTexture);
        }

        [Button]
        public void SetValues(float actual, float target)
        {
            // if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            var rect = rectTransform.rect;

            Value = actual;
            Target = target;
            if (middle.HasValue)
                middle.Value.SetTexture(actual <= target ? damageTexture : healTexture);

            if (vertLeft is not { Length: 4 } ) vertLeft = new Vector3[4];
            if (vertMiddle is not { Length: 4 } ) vertMiddle = new Vector3[4];
            if (vertRight is not { Length: 4 } ) vertRight = new Vector3[4];
        
            //   0 - 1 - 2 - 3
            //  /   /   /   /
            // 4 - 5 - 6 - 7

            var v3 = (Vector3)rect.max;
            var v4 = (Vector3)rect.min;
            var v0 = new Vector3(v4.x, v3.y);
            var v7 = new Vector3(v3.x, v4.y);

            v0 = Vector3.LerpUnclamped(v0, v3, cornerOffset);
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

            left.Value?.SetVertices(vertLeft);
            middle.Value?.SetVertices(vertMiddle);
            right.Value?.SetVertices(vertRight);
        }
    }
}