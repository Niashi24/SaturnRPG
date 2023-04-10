using System.Collections.Generic;
using LS.SearchWindows;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class PartyMember : ScriptableObject
	{
		[field: SerializeField]
		public string Name { get; private set; }
		
		[field: SerializeField]
		public List<BattleMove> Moves { get; private set; }
		
		[field: SerializeField, Required]
		public BattleAttackChooser BattleAttackChooser { get; private set; }
		
		[field: SerializeField]
		public BattleStats Stats { get; private set; }
		
		[field: SerializeField, AssetSearch]
		public PartyMemberVisual VisualPrefab { get; private set; }

		public abstract int GetStartHP();
		public abstract int GetStartMP();
	}
}