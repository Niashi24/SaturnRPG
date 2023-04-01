using System.Collections.Generic;
using UnityEngine;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Party/Party")]
	public class BattleParty : ScriptableObject
	{
		[field: SerializeField] public List<PartyMember> PartyMembers { get; private set; }
	}
}