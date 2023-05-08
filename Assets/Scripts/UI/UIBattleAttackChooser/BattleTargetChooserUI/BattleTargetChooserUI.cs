using System;
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
			throw new NotImplementedException();
		}

		public void ResetSelection()
		{
			throw new NotImplementedException();
		}

		public void SetSelection(ITargetable previousTarget)
		{
			throw new NotImplementedException();
		}
	}
}