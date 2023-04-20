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
        [field: SerializeField, Tooltip("Time (seconds) between ending moving the lead value and starting to move the follow value.")]
        public float SetDelay { get; private set; } = 0.5f;

        
        public float LeadTime = 0.1f;
        private float _leadVel = 0;
        public float FollowTime = 0.1f;
        private float _followVel = 0;

        private float _timer = 0;

        [ShowInInspector, ReadOnly] private float _value, _target;

        [ShowInInspector, ReadOnly]
        public float Value { get; private set; }

        [SerializeField]
        private ObjectReference<ValueBarDisplay> valueBarDisplay;

        private void Update()
        {
            if (_target != _value)
                UpdateLead();
            else if (_timer > 0)
                _timer = Mathf.MoveTowards(_timer, 0, Time.deltaTime);
            else
                UpdateFollow();
        }

        private void UpdateLead()
        {
            if (Mathf.Abs(_target - _value) < 0.01f)
                _value = _target;
            else
                _value = Mathf.SmoothDamp(_value, _target, ref _leadVel, LeadTime);

            UpdateValue();
        }

        private void UpdateFollow()
        {
            if (Value == _value) return;
            
            if (Mathf.Abs(Value - _value) < 0.01f)
                Value = _value;
            else
                Value = Mathf.SmoothDamp(Value, _value, ref _followVel, FollowTime);
            
            UpdateValue();
        }

        private void UpdateValue()
        {
            valueBarDisplay.Value?.SetValues(_value, Value);
        }

        [Button]
        public void SetValue(float value)
        {
            _target = Mathf.Clamp01(value);
            ResetTimer();
        }

        private void ResetTimer()
        {
            _timer = SetDelay;
        }
    }
}
