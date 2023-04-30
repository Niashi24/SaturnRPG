using System.Collections;
using System.Collections.Generic;
using LS.SearchWindows;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG
{
    [CreateAssetMenu(menuName = "Battle/Background")]
    public class BattleBackground : ScriptableObject
    {
        [field: SerializeField, Required]
        public Sprite Wall { get; private set; }

        [field: SerializeField, Required]
        public Texture Floor { get; private set; }

        [field: SerializeField, AssetSearch]
        public GameObject BackgroundPrefab { get; private set; }
    }
}
