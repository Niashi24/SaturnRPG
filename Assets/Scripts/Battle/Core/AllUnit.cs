using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class AllUnit : MonoBehaviour, ITargetable, I3DViewable
	{
		public int HP => 0;
		public int MP => 0;
		
		[field: SerializeField]
		public string Name { get; private set; }
		
		public List<StatusCondition> StatusConditions { get; private set; } = new ();
		public BattleStats BaseStats => new BattleStats();

		public I3DViewable Viewable3D => this;

		private List<BattleUnit> _activeUnits;

		public void SetActiveUnits(List<BattleUnit> ActiveUnits)
		{
			_activeUnits = ActiveUnits;
		}

		public bool CanBeAttacked()
		{
			return _activeUnits != null && _activeUnits.Count != 0;
		}

		public BattleStats GetBattleStats()
		{
			Debug.LogError("Error! Tried to get battle stats of AllUnit", this);
			return new BattleStats();
		}

		public List<BattleMove> GetAvailableMoves(BattleUnit user, BattleContext context)
		{
			Debug.LogError("Error! Tried to get available moves of AllUnit.", this);
			return new List<BattleMove>();
		}

		public async UniTask<BattleAttack> ChooseAttack(BattleContext context)
		{
			await UniTask.CompletedTask;
			return new BattleAttack();
		}

		public async UniTask AddStatusCondition(BattleContext context, StatusCondition statusCondition)
		{
			if (_activeUnits == null)
			{
				Debug.LogError("Active Units not set in All Unit", this);
				await UniTask.CompletedTask;
				return;
			}

			await UniTask.WhenAll(_activeUnits.Select(x => x.AddStatusCondition(context, statusCondition)));
		}

		public async UniTask DealDamage(int damage)
		{
			if (_activeUnits == null)
			{
				Debug.LogError("Active Units not set in All Unit", this);
				await UniTask.CompletedTask;
				return;
			}

			await UniTask.WhenAll(_activeUnits.Select(x => x.DealDamage(damage)));
		}

		public async UniTask UseMP(int mp)
		{
			if (_activeUnits == null)
			{
				Debug.LogError("Active Units not set in All Unit", this);
				await UniTask.CompletedTask;
				return;
			}

			await UniTask.WhenAll(_activeUnits.Select(x => x.UseMP(mp)));
		}

		public Vector3 GetPosition()
		{
			if (_activeUnits == null || _activeUnits.Count == 0) return Vector3.zero;

			return _activeUnits.Select(x => x.Viewable3D.GetPosition()).Average();
		}
	}
}