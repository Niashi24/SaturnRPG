using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public abstract class ActionMoveComponent : BattleMoveComponent
	{
		[SerializeField, Required]
		private Transform playerSpawnLocation;

		public override async UniTask PlayAttack(BattleContext context, BattleAttack attack)
		{
			var targetPartyMember = attack.Target.GetPartyMember();
			// Use the corresponding action component (if it exists) if it has a party member
			// Otherwise use default action component
			var actionComponent = targetPartyMember.Enabled
				? context.PlayerActionInfo[targetPartyMember.Value]
				: context.PlayerActionInfo.DefaultActionComponent;

			var playerActionComponent = Instantiate(actionComponent, playerSpawnLocation.position,
				Quaternion.identity, transform);

			await PlayMove(context, attack, playerActionComponent);

			Destroy(playerActionComponent.gameObject);
		}

		protected abstract UniTask PlayMove(BattleContext context, BattleAttack attack,
			PlayerActionComponent playerActionComponent);
	}
}