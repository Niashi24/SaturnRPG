using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Battle Move")]
	public class BattleMove : ScriptableObject
	{
		[SerializeField, Required] private BattleMoveComponent movePrefab;

		[field: SerializeField] public string MoveName { get; private set; }

		[field: SerializeField, TextArea(3, int.MaxValue)]
		public string MoveDescription;
		
		[field: SerializeField] public int MPCost { get; private set; }

		public async UniTask PlayMove(BattleContext context, BattleAttack attack)
		{
			var battleMove = Instantiate(movePrefab, Vector3.zero, Quaternion.identity);
			await battleMove.PlayAttack(context, attack);
			Destroy(battleMove.gameObject);
		}

		public bool CanBeUsed(BattleContext context, PartyMemberBattleUnit user)
		{
			return user.MP >= MPCost && movePrefab.CanBeUsed(context, user);
		}
	}
}