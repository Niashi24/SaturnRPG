using UnityEngine;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Party/Enemy Party Member")]
	public class EnemyPartyMember : PartyMember
	{
		public override int GetStartHP() => Stats.HP;

		public override int GetStartMP() => Stats.MP;
	}
}