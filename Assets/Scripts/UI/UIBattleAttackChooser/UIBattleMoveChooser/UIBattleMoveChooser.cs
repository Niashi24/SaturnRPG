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
		[ValidateInput(nameof(TabsValid))]
		private List<UITab> tabs;

		[SerializeField]
		private List<UIMoveSelection> moveSelections;

		public int CurrentTabIndex { get; private set; } = 0;
		public int CurrentSelectionIndex { get; private set; } = 0;
		private MoveType CurrentType => (MoveType)CurrentTabIndex;

		private readonly List<BattleMove> _currentMoves = new();
		private readonly Dictionary<MoveType, List<BattleMove>> _typeToMoves = new();
		// Set when calling ChooseMove or RedoMoveChoice
		private BattleUnit _currentUnit;
		private BattleContext _currentContext;
		// Set when move is selected by user
		private BattleMove _selectedMove;

		private bool _setup;
		
		private void Awake()
		{
			if (!_setup) Setup();
		}

		private void Setup()
		{
			foreach (var tab in tabs)
			{
				var temp = tab;
				temp.OnSelect += () => OnSelectTab(temp);
			}

			foreach (var selection in moveSelections)
			{
				var temp = selection;
				selection.OnSelection += () => OnSelectMove(temp);
			}

			foreach (MoveType type in Enum.GetValues(typeof(MoveType)))
			{
				_typeToMoves[type] = new List<BattleMove>();
			}

			_setup = true;
		}

		public void PopulateMoves(BattleContext context, BattleUnit unit)
		{
			_selectedMove = null;

			_currentUnit = unit;
			_currentContext = context;
			var moves = unit.GetAvailableMoves(context);
			
			_currentMoves.AddRange(moves);

			// Separate into tabs by move type
			foreach (var move in _currentMoves)
				_typeToMoves[move.MoveType].Add(move);
			
			SetActiveTabIndex(0);
		}

		public async UniTask<BattleMove> RedoMoveChoice(BattleContext context, BattleUnit unit, BattleMove previous)
		{
			if (!_setup) Setup();
			ResetSelection();
			PopulateMoves(context, unit);
			SetSelection(previous);
			gameObject.SetActive(true);
			while (_selectedMove == null)
				await UniTask.Yield();
			
			gameObject.SetActive(false);
			return _selectedMove;
		}

		public async UniTask<BattleMove> ChooseMove(BattleContext context, BattleUnit unit)
		{
			if (!_setup) Setup();
			ResetSelection();
			PopulateMoves(context, unit);
			gameObject.SetActive(true);
			while (_selectedMove == null)
				await UniTask.Yield();

			gameObject.SetActive(false);
			return _selectedMove;
		}

		public void ResetSelection()
		{
			tabs[CurrentTabIndex].SetActive(false);
			CurrentTabIndex = 0;
			CurrentSelectionIndex = 0;
			tabs[CurrentTabIndex].SetActive(true);
			
			_currentMoves.Clear();
			foreach (var (_, moves) in _typeToMoves)
			{
				moves.Clear();
			}

			foreach (var selection in moveSelections)
				selection.ResetSelection();
		}

		public void SetSelection(BattleMove previousMoveBase)
		{
			int typeIndex = (int)previousMoveBase.MoveType;
			if (typeIndex < tabs.Count)
			{
				Debug.LogWarning($"Could not set selection: Move type is out of bounds.", this);
				return;
			}
			
			OnSelectTab(tabs[typeIndex]);
			
			
		}

		public void SetActiveSelectionIndex(int index)
		{
			if (index == CurrentSelectionIndex) return;
			
			moveSelections[CurrentSelectionIndex].Exit();
			
			if (index > moveSelections.Count) index = 0;
			if (index < 0) index = moveSelections.Count - 1;

			CurrentSelectionIndex = index;
			moveSelections[CurrentSelectionIndex].Enter();
		}

		public void SetActiveTabIndex(int index)
		{
			if (index == CurrentTabIndex) return;

			tabs[CurrentTabIndex].Exit();

			if (index > tabs.Count) index = 0;
			if (index < 0) index = tabs.Count - 1;

			CurrentTabIndex = index;
			tabs[CurrentTabIndex].Enter();
		}

		public void SelectCurrentSelection()
		{
			var moves = _typeToMoves[CurrentType];
			if (CurrentSelectionIndex >= moves.Count || CurrentSelectionIndex < 0)
			{
				if (CurrentSelectionIndex == 0) return;  // Sometimes there's no moves to select and that's OK
				// If it tried to select a move out of range otherwise, that's bad
				Debug.LogError("Tried to select an out of range move.", this);
				return;
			}

			_selectedMove = moves[CurrentSelectionIndex];
		}

		private void OnSelectTab(UITab tab)
		{
			int tabIndex = tabs.IndexOf(tab);
			if (tabIndex == -1) return;
			if (tabIndex == CurrentTabIndex) return;

			tabs[CurrentTabIndex].Deselect();
			CurrentTabIndex = tabIndex;
			
			Debug.Log($"Selected tab #{tabIndex}");
			
			// TODO: Switch tabs
			SetActiveSelectionIndex(0);
			DisplayMovesOfType((MoveType)tabIndex);
		}

		private void OnSelectMove(UIMoveSelection selection)
		{
			if (selection.Move == null)
			{
				Debug.LogError("Selected tab with no move", selection);
				return;
			}
			_selectedMove = selection.Move;
		}

		private void DisplayMovesOfType(MoveType moveType)
		{
			var moves = _typeToMoves[moveType];
			moveSelections.ForEach(x => x.gameObject.SetActive(false));
			for (int i = 0; i < moves.Count && i < moveSelections.Count; i++)
			{
				moveSelections[i].SetMove(moves[i], _currentContext, _currentUnit);
				moveSelections[i].gameObject.SetActive(true);
			}
		}

		private bool TabsValid()
		{
			if (tabs.Count < 3) return false;

			return true;
		}
	}
}