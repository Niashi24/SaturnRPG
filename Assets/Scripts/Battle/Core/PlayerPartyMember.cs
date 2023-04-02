using UnityEngine;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Party/Player Party Member")]
	public class PlayerPartyMember : PartyMember
	{
		[field: SerializeField, Min(0)]
		public int HP { get; private set; }
		
		[field: SerializeField, Min(0)]
		public int MP { get; private set; }
		
		public override int GetStartHP() => HP;
		public override int GetStartMP() => MP;
	}
}