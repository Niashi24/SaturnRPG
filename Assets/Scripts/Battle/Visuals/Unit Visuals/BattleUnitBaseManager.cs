using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle.Visuals;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace SaturnRPG.Battle.Visuals
{
	public class BattleUnitBaseManager : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleUnitPlacers3D battleUnitPlacers3D;

		[SerializeField, Required]
		private BattleUnitBase basePrefab;
		
		private ObjectPool<BattleUnitBase> _unitBasePool;

		private List<BattleUnitBase> _basesInUse;

		private void OnEnable()
		{
			_unitBasePool = basePrefab.CreateMonoPool(parent: transform);
			_basesInUse = new();
			BattleManager.OnSetSingleton += manager =>
			{
				manager.OnBattleStateChange.Subscribe(CleanUpWhenDone);
			};

			battleUnitPlacers3D.OnSetAnchor += AddBaseToAnchor;
		}

		private void AddBaseToAnchor((BattleUnit, Transform) obj)
		{
			var (unit, trans) = obj;

			var unitBase = _unitBasePool.Get();

			unitBase.SetAnchorUnit(unit, trans);
			
			_basesInUse.Add(unitBase);
		}

		private UniTask CleanUpWhenDone(BattleState state)
		{
			if (state != BattleState.End) return UniTask.CompletedTask;
			
			foreach (var unitBase in _basesInUse)
			{
				_unitBasePool.Release(unitBase);
			}
			
			_basesInUse.Clear();

			return UniTask.CompletedTask;
		}
	}
}