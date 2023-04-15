using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle
{

	public class NullTarget : ITargetable, I3DViewable
	{
		public static NullTarget I { get; private set; } = new();
		
		public int HP => 0;
		public int MP => 0;
		public string Name => "";
		public List<StatusCondition> StatusConditions { get; private set; } = new();
		public BattleStats BaseStats { get; }

		public I3DViewable Viewable3D => this;

		public bool CanBeAttacked() => false;

		public BattleStats GetBattleStats() => BaseStats;

		public UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition)
			=> UniTask.CompletedTask;

		public UniTask DealDamage(int damage) => UniTask.CompletedTask;

		public UniTask UseMP(int mp) => UniTask.CompletedTask;

		public Vector3 GetPosition() => Vector3.zero;
	}
}