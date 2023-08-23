using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.UI
{
	public class UIBattleMoveChooser : MonoBehaviour
	{
		[SerializeField]
		// [ValidateInput(nameof(TabsValid))]
		private List<UITab> tabs;

		[SerializeField]
		private List<UIMoveSelection> moveSelections;
		
		[field: SerializeField, Required]
		public GameObject VisualRoot { get; private set; }

		// Current selection stuff
		private int _tabIndex = 0;
		private int _selectionIndex = 0;
		private MoveType CurrentType => (MoveType)_tabIndex;
		// View Enemy Status stuff
		private bool _shouldViewEnemyStatus = false;
		private I3DViewable _previousViewable;

		private readonly List<BattleMove> _currentMoves = new();
		private readonly Dictionary<MoveType, List<BattleMove>> _typeToMoves = new();

		public List<BattleMove> ActiveMoves => _typeToMoves[CurrentType];
		// Set when calling ChooseMove or RedoMoveChoice
		private BattleUnit _currentUnit;
		private BattleContext _currentContext;
		// Set when move is selected by user
		private BattleMove _selectedMove;

		private bool _setup;
		private bool _canceled;
		public bool Active { get; private set; }

		public event Action<BattleUnit> OnStartSelection;
		public event Action OnEndSelection;
		/// <summary>
		/// An event triggered when changing to a new move or starting a new move selection.
		/// If there are no moves available, the BattleMove will be null.
		/// </summary>
		public event Action<BattleMove> OnHighlightMove;

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

		public UniTask<BattleMove> ChooseMove(BattleContext context, BattleUnit unit)
		{
			SetupMoves(context, unit);

			return WaitForMove(context, unit);
		}

		private async UniTask<BattleMove> WaitForMove(BattleContext context, BattleUnit unit)
		{
			OnStartSelection?.Invoke(unit);
			
			OnHighlightMove?.Invoke(_currentMoves.GetIfInRange(_selectionIndex));
			
			while (true)
			{
				if (_canceled)
				{
					Active = false;
					OnEndSelection?.Invoke();
					gameObject.SetActive(false);
					return null;
				}

				if (_selectedMove != null) break;
				await UniTask.Yield(context.BattleCancellationToken);
			}

			Active = false;
			gameObject.SetActive(false);
			OnEndSelection?.Invoke();
			
			return _selectedMove;
		}

		public UniTask<BattleMove> RedoMoveChoice(BattleContext context, BattleUnit unit, BattleMove previous)
		{
			SetupMoves(context, unit);

			SetCurrentMove(previous);

			return WaitForMove(context, unit);
		}

		private void SetCurrentMove(BattleMove previous)
		{
			var moveType = previous.MoveType;
			if (!_typeToMoves.ContainsKey(moveType)) return;
			int moveIndex = _typeToMoves[moveType].FirstIndexWhere(x => x == previous);
			if (moveIndex == -1) return;
			
			SetTabIndex((int)moveType);
			SetSelectionIndex(moveIndex);
		}

		[Button]
		public void IncrementTab(int i)
		{
			if (!Active) return;
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
			if (!Active) return;
			if (i == 0) return;
			if (_currentMoves.Count == 0) return;

			var ActiveMoves = this.ActiveMoves;
			if (ActiveMoves.Count == 0) return;

			int newIndex = _selectionIndex + i;
			if (newIndex >= ActiveMoves.Count)
				newIndex = 0;
			if (newIndex < 0)
				newIndex = ActiveMoves.Count - 1;
			
			SetSelectionIndex(newIndex);
			//TODO:
		}

		public void SelectActiveSelection()
		{
			if (!Active) return;
			if (_currentMoves.Count == 0) return;
			if (_typeToMoves[CurrentType].Count == 0) return;
			
			SelectSelection(moveSelections[_selectionIndex]);
		}

		private void SetActiveTab(UITab tab)
		{
			if (!Active) return;
			int index = tabs.IndexOf(tab);
			if (index == -1) return;
			
			SetTabIndex(index);
		}

		public void SetActiveSelection(UIMoveSelection selection)
		{
			if (!Active) return;
			int index = moveSelections.IndexOf(selection);
			if (index == -1) return;
			
			SetSelectionIndex(index);
		}

		[Button]
		public void CancelSelection()
		{
			if (!Active) return;
			
			_canceled = true;
			Active = false;
		}

		public void SelectSelection(UIMoveSelection selection)
		{
			if (!Active) return;
			if (!selection.Usable) return;
			if (selection.Move == null) return;

			_selectedMove = selection.Move;
			Active = false;
		}

		private void SetupMoves(BattleContext context, BattleUnit unit)
		{
			if (!_setup) Setup();
			ResetSelection();
			_selectedMove = null;
			_currentUnit = unit;
			_canceled = false;
			_currentContext = context;
			
			_currentMoves.AddRange(unit.GetAvailableMoves(context));

			foreach (var move in _currentMoves)
			{
				_typeToMoves[move.MoveType].Add(move);
			}
			
			Active = true;
			SetTabIndex(0);
			SetSelectionIndex(0);
			gameObject.SetActive(true);
		}

		private void SetTabIndex(int tabIndex)
		{
			if (tabIndex < 0) return;
			if (tabIndex >= tabs.Count) return;
			
			tabs[_tabIndex].SetActive(false);
			tabs[tabIndex].SetActive(true);

			_tabIndex = tabIndex;
			
			DisplayMovesOfType(CurrentType);
			OnHighlightMove?.Invoke(ActiveMoves.GetIfInRange(_selectionIndex));
		}

		private void SetSelectionIndex(int i)
		{
			if (i < 0) return;
			if (i >= moveSelections.Count) return;
			
			moveSelections[_selectionIndex].SetActive(false);
			moveSelections[i].SetActive(true);
			_selectionIndex = i;
			OnHighlightMove?.Invoke(ActiveMoves[_selectionIndex]);
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

		[Button]
		public void SetViewEnemyStatus(bool shouldViewEnemyStatus)
		{
			if (_shouldViewEnemyStatus && !shouldViewEnemyStatus)
				GoBackToMoves();
			else if (!_shouldViewEnemyStatus && shouldViewEnemyStatus)
				GoToEnemyStatus();

			_shouldViewEnemyStatus = shouldViewEnemyStatus;
		}

		private void GoBackToMoves()
		{
			_currentContext.BattleCamera.SetTarget(_previousViewable);
			_currentContext.EnemyUnitManager.SetAllHealthBarsActive(false);
			VisualRoot.SetActive(true);
			Active = true;
		}

		private void GoToEnemyStatus()
		{
			_previousViewable = _currentContext.BattleCamera.Target;
			_currentContext.BattleCamera.SetTarget(_currentContext.EnemyUnitManager.AllTargetable as I3DViewable);
			_currentContext.EnemyUnitManager.SetAllHealthBarsActive(true);
			VisualRoot.SetActive(false);
			Active = false;
		}
	}
}