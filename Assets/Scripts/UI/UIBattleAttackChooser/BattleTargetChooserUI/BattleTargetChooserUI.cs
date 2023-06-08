using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class BattleTargetChooserUI : MonoBehaviour
	{
		[SerializeField]
		private List<UITarget> uiTargets;

		[ShowInInspector, ReadOnly]
		public readonly List<UITarget> ActiveTargets = new();

		public event Action<List<UITarget>> OnStartSelection;
		public event Action OnEndSelection;

		private ITargetable _selection;
		private bool _setup;
		private bool _active;
		private int _targetIndex;
		private bool _canceled;

		private void Setup()
		{
			foreach (var target in uiTargets)
			{
				var temp = target;

				temp.OnEnter += () => SetSelectedTarget(temp);
				temp.OnSelect += () => SelectTarget(temp);
			}
		}

		public async UniTask<ITargetable> ChooseTarget(BattleContext context, BattleUnit unit, BattleMove move)
		{
			LoadTargetSelections(context, unit, move);

			return await WaitForTargetable(context);
		}

		public async UniTask<ITargetable> ReChooseTarget(BattleContext context, BattleUnit unit, BattleMove move, ITargetable previousTarget)
		{
			LoadTargetSelections(context, unit, move);

			SetTargetable(previousTarget);

			return await WaitForTargetable(context);
		}

		private void SetTargetable(ITargetable previousTarget)
		{
			int index = ActiveTargets.FirstIndexWhere(x => x.Targetable == previousTarget);
			if (index == -1) return;
			
			SetSelectionIndex(index);
		}

		private async Task<ITargetable> WaitForTargetable(BattleContext context)
		{
			OnStartSelection?.Invoke(ActiveTargets);

			while (true)
			{
				if (_canceled)
				{
					_active = false;
					OnEndSelection?.Invoke();
					gameObject.SetActive(false);
					return null;
				}

				if (_selection != null) break;
				await UniTask.Yield(context.BattleCancellationToken);
			}

			_active = false;
			OnEndSelection?.Invoke();
			gameObject.SetActive(false);
			return _selection;
		}

		private void LoadTargetSelections(BattleContext context, BattleUnit unit, BattleMove move)
		{
			if (!_setup) Setup();
			_active = true;
			_canceled = false;
			_selection = null;
			gameObject.SetActive(true);
			var targetables = move.GetTargetables(unit, context);
			targetables.Sort((a, b) => a.Viewable3D.GetPosition().x > b.Viewable3D.GetPosition().x ? 1 : -1);

			ActiveTargets.ForEach(x => x.gameObject.SetActive(false));
			ActiveTargets.Clear();
			_targetIndex = 0;
			for (int i = 0; i < targetables.Count && i < uiTargets.Count; i++)
			{
				uiTargets[i].SetTarget(targetables[i], targetables[i].CanBeAttacked());
				uiTargets[i].gameObject.SetActive(true);
				ActiveTargets.Add(uiTargets[i]);
			}
		}

		public void SetSelection(BattleContext context, BattleUnit unit, ITargetable previousTarget)
		{
			throw new NotImplementedException();
		}

		private void SetSelectedTarget(UITarget uiTarget)
		{
			if (!_active) return;
			
			var index = ActiveTargets.IndexOf(uiTarget);
			if (index == -1) return;

			SetSelectionIndex(index);
		}

		private void SetSelectionIndex(int index)
		{
			if (ActiveTargets.Count == 0) return;

			if (index < 0) index = ActiveTargets.Count - 1;
			if (index >= ActiveTargets.Count) index = 0;

			ActiveTargets[_targetIndex].SetActive(false);
			ActiveTargets[index].SetActive(true);
		}

		[Button]
		public void SelectCurrentTarget()
		{
			if (!_active) return;
			if (ActiveTargets.Count == 0) return;
			
			SelectTarget(ActiveTargets[_targetIndex]);
		}

		[Button]
		public void CancelSelection()
		{
			_canceled = true;
			_active = false;
		}

		private void SelectTarget(UITarget uiTarget)
		{
			if (uiTarget.Targetable == null) return;
			if (!uiTarget.Usable) return;
			_selection = uiTarget.Targetable;
			_active = false;
		}
	}
}