using System;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class UnitDamage : MonoBehaviour
	{
		[SerializeField, Required]
		private BattleUnit battleUnit;

		private void OnEnable()
		{
			battleUnit.OnHPChange.Subscribe(PrintDamage);
		}

		private void OnDisable()
		{
			battleUnit.OnHPChange.Unsubscribe(PrintDamage);
		}

		private async UniTask PrintDamage(int HP, int oldHP)
		{
			int dHP = HP - oldHP;
			
			if (dHP > 0)
				Debug.Log($"{battleUnit.Name} recovered {dHP} HP!");
			else
				Debug.Log($"{battleUnit.Name} took {-dHP} damage!");
			
			if (battleUnit.HP > 0)
				Debug.Log($"{battleUnit.Name} is at {battleUnit.HP} HP!");
			else
				Debug.Log($"{battleUnit.Name} was knocked out!");

			await UniTask.Delay(1000);
		}
	}
}