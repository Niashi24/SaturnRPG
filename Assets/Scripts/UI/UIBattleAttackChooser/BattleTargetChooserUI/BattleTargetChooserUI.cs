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
		[SerializeField]
		private List<UITarget> uiTargets;

		public readonly List<UITarget> ActiveTargets = new();
		
		private ITargetable _selection;
		private bool _setup;
		private int _targetIndex;

		private void Setup()
		{
			
		}
		
		public async UniTask<ITargetable> ChooseTarget(BattleContext context, BattleUnit unit, BattleMove move)
		{
			if (!_setup) Setup();
			var targetables = move.GetTargetables(unit, context);

			ActiveTargets.ForEach(x => x.gameObject.SetActive(false));
			ActiveTargets.Clear();
			_targetIndex = 0;
			for (int i = 0; i < targetables.Count && i < uiTargets.Count; i++)
			{
				uiTargets[i].SetTarget(targetables[i], targetables[i].CanBeAttacked());
				uiTargets[i].gameObject.SetActive(true);
				ActiveTargets.Add(uiTargets[i]);
			}

			while (_selection == null)
				await UniTask.Yield(context.BattleCancellationToken);

			return _selection;
		}

		public void SetSelection(BattleContext context, BattleUnit unit, ITargetable previousTarget)
		{
			throw new NotImplementedException();
		}

		private void SetSelectedTarget(UITarget uiTarget)
		{
			
		}

		private void ResetSelection()
		{
			throw new NotImplementedException();
		}
	}
}