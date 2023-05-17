using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.UI
{
	public class UIBattleMoveChooser : MonoBehaviour
	{
		[SerializeField]
		// [ValidateInput(nameof(TabsValid))]
		private List<UITab> tabs;

		[SerializeField]
		private List<UIMoveSelection> moveSelections;

		[SerializeField]
		private MoveType defaultType = MoveType.Attack;

		private int _tabIndex = 0;
		private int _selectionIndex = 0;
		private MoveType CurrentType => (MoveType)_tabIndex;

		private readonly List<BattleMove> _currentMoves = new();
		private readonly Dictionary<MoveType, List<BattleMove>> _typeToMoves = new();
		// Set when calling ChooseMove or RedoMoveChoice
		private BattleUnit _currentUnit;
		private BattleContext _currentContext;
		// Set when move is selected by user
		private BattleMove _selectedMove;

		private bool _setup;
		private bool _active;
		
		private void Awake()
		{
			if (!_setup) Setup();
		}

		private void Setup()
		{
			foreach (var tab in tabs)
			{
				var temp = tab;
				temp.OnSelect += () => SetActiveTab(temp);
			}

			foreach (var selection in moveSelections)
			{
				var temp = selection;
				temp.SetActive(false);
				temp.gameObject.SetActive(false);
				
				temp.OnEnter += () => SetActiveSelection(temp);
				selection.OnSelection += () => SelectSelection(temp);
			}

			foreach (MoveType type in Enum.GetValues(typeof(MoveType)))
			{
				_typeToMoves[type] = new List<BattleMove>();
			}

			_setup = true;
		}

		public async UniTask<BattleMove> ChooseMove(BattleContext context, BattleUnit unit)
		{
			if (!_setup) Setup();
			ResetSelection();
			SetupMoves(context, unit);
			_active = true;
			SetTabIndex(0);
			SetSelectionIndex(0);
			gameObject.SetActive(true);

			while (_selectedMove == null)
				await UniTask.Yield(context.BattleCancellationToken);
			
			_active = false;
			gameObject.SetActive(false);

			var tempMove = _selectedMove;
			_selectedMove = null;
			return tempMove;
		}

		public async UniTask<BattleMove> RedoMoveChoice(BattleContext context, BattleUnit unit, BattleMove previous)
		{
			if (!_setup) Setup();
			SetupMoves(context, unit);
			_active = true;
			

			while (_selectedMove == null)
				await UniTask.Yield(context.BattleCancellationToken);

			_active = false;
			return _selectedMove;
		}

		[Button]
		public void IncrementTab(int i)
		{
			if (!_active) return;
			if (i == 0) return;
			//TODO:

			int newIndex = _tabIndex + i;
			if (newIndex >= tabs.Count)
				newIndex = 0;
			if (newIndex < 0)
				newIndex = tabs.Count - 1;

			SetTabIndex(newIndex);
		}

		[Button]
		public void IncrementSelection(int i)
		{
			if (!_active) return;
			if (i == 0) return;
			if (_currentMoves.Count == 0) return;
			var currentTypeMoves = _typeToMoves[CurrentType];
			
			if (currentTypeMoves.Count == 0) return;

			int newIndex = _selectionIndex + i;
			if (newIndex >= currentTypeMoves.Count)
				newIndex = 0;
			if (newIndex < 0)
				newIndex = currentTypeMoves.Count - 1;
			
			SetSelectionIndex(newIndex);
			//TODO:
		}

		public void SelectActiveSelection()
		{
			if (!_active) return;
			if (_currentMoves.Count == 0) return;
			if (_typeToMoves[CurrentType].Count == 0) return;
		}

		private void SetActiveTab(UITab tab)
		{
			if (!_active) return;
			int index = tabs.IndexOf(tab);
			if (index == -1) return;
			
			SetTabIndex(index);
		}

		public void SetActiveSelection(UIMoveSelection selection)
		{
			if (!_active) return;
			int index = moveSelections.IndexOf(selection);
			if (index == -1) return;
			
			SetSelectionIndex(index);
		}

		private void SelectSelection(UIMoveSelection selection)
		{
			if (!selection.Usable) return;
			if (selection.Move == null) return;

			_selectedMove = selection.Move;
		}

		private void SetupMoves(BattleContext context, BattleUnit unit)
		{
			_selectedMove = null;
			_currentUnit = unit;
			
			_currentMoves.AddRange(unit.GetAvailableMoves(context));

			foreach (var move in _currentMoves)
			{
				_typeToMoves[move.MoveType].Add(move);
			}
		}

		private void SetTabIndex(int tabIndex)
		{
			if (tabIndex < 0) return;
			if (tabIndex >= tabs.Count) return;
			
			tabs[_tabIndex].SetActive(false);
			tabs[tabIndex].SetActive(true);

			_tabIndex = tabIndex;
			
			DisplayMovesOfType(CurrentType);
		}

		private void SetSelectionIndex(int i)
		{
			if (i < 0) return;
			if (i >= moveSelections.Count) return;
			
			moveSelections[_selectionIndex].SetActive(false);
			moveSelections[i].SetActive(true);
			_selectionIndex = i;
		}

		private void DisplayMovesOfType(MoveType type)
		{
			foreach (var selection in moveSelections)
			{
				selection.ResetSelection();
				selection.gameObject.SetActive(false);
			}
			
			var movesOfType = _typeToMoves[type];
			for (int i = 0; i < movesOfType.Count && i < moveSelections.Count; i++)
			{
				BattleMove move = movesOfType[i];
				bool usable = move.CanBeUsed(_currentContext, _currentUnit);
				moveSelections[i].SetMove(movesOfType[i], usable);
				moveSelections[i].gameObject.SetActive(true);
			}
			
			// TODO
		}

		private void ResetSelection()
		{
			SetTabIndex(0);
			SetSelectionIndex(0);

			foreach (var selection in moveSelections)
			{
				selection.ResetSelection();
				selection.SetActive(false);
			}

			_currentContext = null;
			_currentUnit = null;

			_currentMoves.Clear();
			foreach (var (_, moves) in _typeToMoves)
			{
				moves.Clear();
			}
			// TODO
		}
	}
}