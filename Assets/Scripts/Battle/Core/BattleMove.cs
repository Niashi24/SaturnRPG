using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LS.SearchWindows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(menuName = "Battle/Battle Move")]
	public class BattleMove : ScriptableObject
	{
		[SerializeField, Required]
		[AssetsOnly, AssetSearch]
		private BattleMoveComponent movePrefab;

		[field: SerializeField]
		public string MoveName { get; private set; }

		[field: SerializeField, TextArea(3, int.MaxValue)]
		public string MoveDescription;
		
		[field: SerializeField]
		public int MPCost { get; private set; }
	
		[field: SerializeField, Tooltip("Systems will avoid displaying Discrete Moves to User")]
		public bool Discrete { get; private set; }
		
		[field: SerializeField, Tooltip("Moves with higher priorities will move first")]
		public int Priority { get; private set; }
		
		[field: SerializeField]
		public MoveType MoveType { get; private set; }

		public async UniTask PlayMove(BattleContext context, BattleAttack attack)
		{
			var battleMove = Instantiate(movePrefab, Vector3.zero, Quaternion.identity);
			await battleMove.PlayAttack(context, attack);
			Destroy(battleMove.gameObject);
		}

		public bool CanBeUsed(BattleContext context, BattleUnit user)
		{
			return user.MP >= MPCost && movePrefab.CanBeUsed(context, user);
		}

		public List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
			=> movePrefab.GetTargetables(user, context);

		public BattleStats GetMoveStats(BattleUnit user, ITargetable target, BattleContext context)
			=> movePrefab.GetMoveStats(user, target, context);

		public IEnumerable<BattleUnit> GetExhaustedUnits(BattleAttack battleAttack, BattleContext context)
			=> movePrefab.GetExhaustedUnitsOfAttack(battleAttack, context);
	}
}