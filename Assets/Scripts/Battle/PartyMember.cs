using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public abstract class PartyMember : ScriptableObject
	{
		[field: SerializeField] public List<BattleMove> Moves { get; private set; }
		
		[field: SerializeField, Required] public BattleAttackChooser BattleAttackChooser { get; private set; }
		
		[field: SerializeField] public BattleStats Stats { get; private set; }
	}
}