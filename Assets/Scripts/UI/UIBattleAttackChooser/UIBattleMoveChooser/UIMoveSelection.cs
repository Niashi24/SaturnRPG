using System;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.UI
{
	public class UIMoveSelection : MonoBehaviour
	{
		[SerializeField, Required, Header("Highlight/Border")]
		private Image border;
		[SerializeField]
		private Color borderUsable, borderUnusable;

		[SerializeField, Required, Header("Text")]
		private Text text;
		[SerializeField]
		private Color textUsable, textUnusable;

		public event Action OnSelection;
		
		public bool Usable { get; private set; }
		public BattleMove Move { get; private set; }

		[Button]
		public void SetUsable(bool usable)
		{
			Usable = usable;
			border.color = Usable ? borderUsable : borderUnusable;
			text.color = Usable ? textUsable : textUnusable;
		}

		public void Enter()
		{
			border.enabled = true;
		}

		public void Exit()
		{
			border.enabled = false;
		}

		public void Select()
		{
			if (Usable)
				OnSelection?.Invoke();
		}

		public void SetMove(BattleMove move, BattleContext context, BattleUnit user)
		{
			text.text = move.MoveName;
			Move = move;
			SetUsable(move.CanBeUsed(context, user));
		}

		public void ResetSelection()
		{
			text.text = string.Empty;
			Move = null;
			SetUsable(false);
		}
	}
}