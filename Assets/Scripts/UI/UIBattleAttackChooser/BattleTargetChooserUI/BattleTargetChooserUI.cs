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
		public event Action<UITarget> OnChangeSelection;
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
			if (move.ShouldAutoTargetFirst)
				return move.GetTargetables(unit, context)[0];
			
			LoadTargetSelections(context, unit, move);

			return await WaitForTargetable(context);
		}

		public async UniTask<ITargetable> ReChooseTarget(BattleContext context, BattleUnit unit, BattleMove move,
			ITargetable previousTarget)
		{
			if (move.ShouldAutoTargetFirst)
				return null;  // If retargets first, then can't go back bc continually auto-choosing first
			
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
			OnChangeSelection?.Invoke(ActiveTargets.GetIfInRange(_targetIndex));

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

			ActiveTargets.ForEach(x =>
			{
				x.SetActive(false);
				x.gameObject.SetActive(false);
			});
			ActiveTargets.Clear();
			for (int i = 0; i < targetables.Count && i < uiTargets.Count; i++)
			{
				uiTargets[i].SetTarget(targetables[i], targetables[i].CanBeAttacked());
				uiTargets[i].gameObject.SetActive(true);
				ActiveTargets.Add(uiTargets[i]);
			}

			SetSelectionIndex(0);
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

			if (ActiveTargets.IsInRange(_targetIndex))
				ActiveTargets[_targetIndex].SetActive(false);
			ActiveTargets[index].SetActive(true);
			_targetIndex = index;

			OnChangeSelection?.Invoke(ActiveTargets.GetIfInRange(_targetIndex));
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

		public void IncrementSelectionIndex(int dI)
		{
			if (dI == 0) return;
			if (!_active) return;

			SetSelectionIndex(_targetIndex + dI);
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