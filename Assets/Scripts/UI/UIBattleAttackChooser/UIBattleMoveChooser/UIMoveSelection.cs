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

		[SerializeField, Required]
		private Text mpCostText;
		
		[SerializeField]
		private Color textUsable, textUnusable;
		
		[SerializeField, Required, Header("Target")]
		private Image targetImage;

		[SerializeField, Required]
		private Sprite singleTarget, multiTarget;

		public event Action OnEnter;
		public event Action OnSelection;
		
		public bool Usable { get; private set; }
		public bool Active { get; private set; }
		public BattleMove Move { get; private set; }

		[Button]
		public void SetUsable(bool usable)
		{
			Usable = usable;
			border.color = Usable ? borderUsable : borderUnusable;
			text.color = mpCostText.color = Usable ? textUsable : textUnusable;
		}

		public void Enter()
		{
			OnEnter?.Invoke();
		}

		public void Exit()
		{
			// border.enabled = false;
		}

		public void Select()
		{
			if (Usable)
				OnSelection?.Invoke();
		}

		public void SetMove(BattleMove move, bool usable)
		{
			text.text = move.MoveName;
			Move = move;
			mpCostText.gameObject.SetActive(move.MPCost != 0);
			mpCostText.text = move.MPCost.ToString();
			targetImage.sprite = move.TargetType switch
			{
				TargetType.Single => singleTarget,
				TargetType.Multiple => multiTarget,
				_ => singleTarget
			};
			SetUsable(usable);
		}

		public void SetActive(bool active)
		{
			Active = active;
			border.enabled = active;
		}

		public void ResetSelection()
		{
			text.text = string.Empty;
			Move = null;
			SetUsable(false);
		}
	}
}