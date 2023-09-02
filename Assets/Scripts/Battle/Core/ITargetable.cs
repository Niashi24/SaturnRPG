using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Camera3D2D;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public interface ITargetable
	{
		int HP { get; }
		int MP { get; }
		string Name { get; }
		List<StatusCondition> StatusConditions { get; }
		BattleStats BaseStats { get; }
		I3DViewable Viewable3D { get; }

		bool CanBeAttacked();
		BattleStats GetBattleStats();
		UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition);
		UniTask DealDamage(int damage);
		UniTask UseMP(int mp);

		Optional<PartyMember> GetPartyMember();

		public static readonly ManualSize DEFAULT_TARGETABLE_SIZE = new ManualSize(new Vector2(32, 32));
		ISize Size => DEFAULT_TARGETABLE_SIZE;
	}
}