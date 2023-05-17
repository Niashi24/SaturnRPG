using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class BattleTargetChooserUI : MonoBehaviour
	{
		public async UniTask<ITargetable> ChooseTarget(BattleContext context, BattleUnit unit, BattleMove move)
		{
			var targetables = move.GetTargetables(unit, context);
			return targetables[0];
			throw new NotImplementedException();
		}

		private void ResetSelection()
		{
			throw new NotImplementedException();
		}

		public void SetSelection(BattleContext context, BattleUnit unit, ITargetable previousTarget)
		{
			throw new NotImplementedException();
		}
	}
}