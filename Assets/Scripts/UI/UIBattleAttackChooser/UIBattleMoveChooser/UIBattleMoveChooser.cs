using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class UIBattleMoveChooser : MonoBehaviour
	{
		public async UniTask<BattleMove> ChooseMove(BattleContext context, BattleUnit unit)
		{
			throw new System.NotImplementedException();
		}

		public void ResetSelection()
		{
			throw new System.NotImplementedException();
		}

		public void SetSelection(BattleMove previousMoveBase)
		{
			throw new System.NotImplementedException();
		}
	}
}