using System.Collections.Generic;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	[CreateAssetMenu(menuName = "Battle/Action/Player Action Info")]
	public class PlayerActionInfo : SerializedScriptableObject
	{
		[SerializeField]
		private Dictionary<PartyMember, PlayerActionComponent> partyMemberToActionComponent;

		[SerializeField, Required]
		private PlayerActionComponent defaultActionComponent;

		public PlayerActionComponent this[PartyMember partyMember]
			=> partyMemberToActionComponent.ContainsKey(partyMember)
				? partyMemberToActionComponent[partyMember]
				: defaultActionComponent;
	}
}