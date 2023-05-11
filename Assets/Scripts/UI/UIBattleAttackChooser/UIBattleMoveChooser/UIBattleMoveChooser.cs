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

		private int _currentTabIndex = 0;

		private readonly List<BattleMove> _currentMoves = new();
		private readonly Dictionary<MoveType, List<BattleMove>> _typeToMoves = new();
		
		public bool Populated { get; private set; }

		private void Awake()
		{
			foreach (var tab in tabs)
			{
				var temp = tab;
				temp.OnSelect += () => OnSelectTab(temp);
			}

			foreach (MoveType type in Enum.GetValues(typeof(MoveType)))
			{
				_typeToMoves[type] = new List<BattleMove>();
			}
		}

		public void PopulateMoves(BattleContext context, BattleUnit unit)
		{
			if (Populated)
				ResetSelection();
			var moves = unit.GetAvailableMoves(context);
			
			_currentMoves.AddRange(moves);

			// Separate into tabs by move type
			foreach (var move in _currentMoves)
				_typeToMoves[move.MoveType].Add(move);

			Populated = true;
			throw new NotImplementedException();
		}

		public UniTask<BattleMove> ChooseMove(BattleContext context, BattleUnit unit)
		{
			
			throw new System.NotImplementedException();
		}

		public void ResetSelection()
		{
			tabs[_currentTabIndex].SetActive(false);
			_currentTabIndex = 0;
			tabs[_currentTabIndex].SetActive(true);
			
			_currentMoves.Clear();
			foreach (var (type, moves) in _typeToMoves)
			{
				moves.Clear();
			}
			Populated = false;
		}

		public void SetSelection(BattleMove previousMoveBase)
		{
			throw new System.NotImplementedException();
		}

		private void OnSelectTab(UITab tab)
		{
			int tabIndex = tabs.IndexOf(tab);
			if (tabIndex == -1) return;
			if (tabIndex == _currentTabIndex) return;
			
			tabs[_currentTabIndex].Deselect();
			_currentTabIndex = tabIndex;
			
			Debug.Log($"Selected tab #{tabIndex}");
			
			// TODO: Switch tabs
		}

		private bool TabsValid()
		{
			if (tabs.Count < 3) return false;

			return true;
		}
	}
}