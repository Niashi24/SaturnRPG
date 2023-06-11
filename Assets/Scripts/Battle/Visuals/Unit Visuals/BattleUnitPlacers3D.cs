using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class BattleUnitPlacers3D : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleUnitManager playerUnitManager, enemyUnitManager;

		[SerializeField]
		private List<Transform> playerAnchors, enemyAnchors;

		public event Action<(BattleUnit, Transform)> OnSetAnchor;

		void OnEnable()
		{
			playerUnitManager.OnSetActiveUnits += SetPlayerAnchors;
			enemyUnitManager.OnSetActiveUnits += SetEnemyAnchors;
		}

		void OnDisable()
		{
			playerUnitManager.OnSetActiveUnits -= SetPlayerAnchors;
			enemyUnitManager.OnSetActiveUnits -= SetEnemyAnchors;
		}

		private void SetPlayerAnchors(List<BattleUnit> activeUnits) => SetUnitAnchors(activeUnits, playerAnchors);
		private void SetEnemyAnchors(List<BattleUnit> activeUnits) => SetUnitAnchors(activeUnits, enemyAnchors);

		private void SetUnitAnchors(List<BattleUnit> activeUnits, List<Transform> anchors)
		{
			for (int i = 0; i < activeUnits.Count && i < anchors.Count; i++)
			{
				activeUnits[i].UnitVisual.SetAnchor(anchors[i]);
				OnSetAnchor?.Invoke((activeUnits[i], anchors[i]));
			}
		}
	}
}