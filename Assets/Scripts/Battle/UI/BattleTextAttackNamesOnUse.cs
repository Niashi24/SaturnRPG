using System;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.UI
{
	public class BattleTextAttackNamesOnUse : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleAttackManager battleAttackManager;

		[SerializeField, Required]
		private BattleText battleText;

		[SerializeField]
		private float attackNameTime = 1f;

		private void Awake()
		{
			battleAttackManager.OnAttack.Subscribe(SetAttackNameOnUse);
		}

		private void OnDestroy()
		{
			battleAttackManager.OnAttack.Unsubscribe(SetAttackNameOnUse);
		}

		private async UniTask SetAttackNameOnUse(BattleAttack attack, BattleContext context)
		{
			battleText.SetTextAndActive(attack.MoveBase.MoveName);
			await UniTask.Delay((attackNameTime * 1000).Round(), cancellationToken: context.BattleCancellationToken);
			battleText.SetActive(false);
		} 
	}
}