using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnit : MonoBehaviour
	{
		public PartyMember PartyMember { get; private set; }

		public void SetPartyMember(PartyMember partyMember)
		{
			this.PartyMember = partyMember;
		}

		public int HP { get; private set; }

		public bool CanAttack { get; private set; } = true;

		public async UniTask<BattleAttack> ChooseAttack(BattleContext context)
		{
			return await PartyMember.BattleAttackChooser.ChooseAttack(context, this);
		}
	}
}