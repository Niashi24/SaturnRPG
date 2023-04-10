using System.Collections;
using System.Collections.Generic;
using LS.Utilities;
using UnityEngine;

namespace SaturnRPG.Utilities
{
    public class ValueAsset<T> : ScriptableObject, IValueSupplier<T>
        where T : Object
    {
        [field: SerializeField]
        public T Value { get; set; }
    }
}
