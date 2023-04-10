using System.Collections.Generic;
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
		
		[field: SerializeField]
		public Animator AnimatorPrefab { get; private set; }

		public abstract int GetStartHP();
		public abstract int GetStartMP();
	}
}