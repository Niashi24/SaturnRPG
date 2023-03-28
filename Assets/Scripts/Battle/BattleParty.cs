using System.Collections.Generic;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleParty : ScriptableObject
	{
		[field: SerializeField] public List<PartyMember> PartyMembers { get; private set; }
	}
}