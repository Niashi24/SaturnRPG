using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using SaturnRPG.Battle;
using SaturnRPG.Camera3D2D;
using SaturnRPG.Utilities.Extensions;

namespace SaturnRPG
{
    public class ShadowPlacer : MonoBehaviour
    {
        [SerializeField, Required]
        private UnitVisual unitVisual;

        [SerializeField, Required]
        private CameraAnchorTransformation cameraAnchorTransformation;

        [SerializeField, Range(0, 1)]
        private float heightToWidthRatio = 0.6f;

        private PartyMemberVisual _partyMemberVisual;

        private Transform _transform;

        private void OnEnable()
        {
            _transform = transform;
            unitVisual.OnSetPartyMemberVisual += SetPartyMemberVisual;
            unitVisual.OnSetAnchor += SetAnchor;
        }

        private void OnDisable()
        {
            unitVisual.OnSetPartyMemberVisual -= SetPartyMemberVisual;
            unitVisual.OnSetAnchor -= SetAnchor;
        }

        private void SetPartyMemberVisual(PartyMemberVisual partyMemberVisual)
        {
            _partyMemberVisual = partyMemberVisual;
        }

        private void SetAnchor(Transform anchor3D)
        {
            cameraAnchorTransformation.Set3DViewable(anchor3D.To3DViewable());
        }

        private void Update()
        {
            UpdateShadowSize();
        }

        private void UpdateShadowSize()
        {
            if (_partyMemberVisual == null) return;
            float width = _partyMemberVisual.Size.Size.x;
            _transform.localScale = new Vector3(width, width * heightToWidthRatio, 1);
        }
    }
}
