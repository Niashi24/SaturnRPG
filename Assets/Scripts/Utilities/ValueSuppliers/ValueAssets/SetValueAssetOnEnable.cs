using System;
using System.Collections;
using System.Collections.Generic;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaturnRPG
{
    public class SetValueAssetOnEnable<T> : MonoBehaviour
        where T : Object
    {
        [SerializeField, Required]
        private T value;

        [SerializeField, Required]
        private ValueAsset<T> asset;

        private void OnEnable()
        {
            if (asset == null) return;
            if (value == null) return;

            asset.Value = value;
        }
    }
}
