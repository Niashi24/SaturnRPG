using System.Collections;
using System.Collections.Generic;
using LS.Utilities;
using SaturnRPG.Rendering.DistortedSprite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
    public class DelayValueBar : MonoBehaviour
    {
        [field: SerializeField, Tooltip("Time (s) before starting to move the health around.")]
        public float SetDelay { get; private set; } = 0.5f;

        [field: SerializeField, Tooltip("Fraction amount per second that the output value will give")]
        public float ValueSpeed { get; private set; } = 2;

        private float _timer = 0;

        [ShowInInspector, ReadOnly] private float _value, _maxValue;

        [ShowInInspector, ReadOnly]
        public float Value { get; private set; }

        [SerializeField]
        private ObjectReference<ValueBarDisplay> valueBarDisplay;

        private void Update()
        {
            if (_timer > 0)
                _timer = Mathf.MoveTowards(_timer, 0, Time.deltaTime);
            else
                UpdateValue();
        }

        private void UpdateValue()
        {
            Value = Mathf.MoveTowards(Value, _value, ValueSpeed * Time.deltaTime);
            
            valueBarDisplay.Value?.SetValues(_value, Value);
        }

        [Button]
        public void SetValue(float value)
        {
            _value = Mathf.Clamp01(value);
            ResetTimer();
        }

        private void ResetTimer()
        {
            _timer = SetDelay;
        }
    }
}
