using System;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public class ActionInput
	{
		public Vector2 MoveDirection;
		public Vector2 AimDirection;
		public bool Shift;
		public bool Primary;
		public bool Secondary;

		public Action OnShift;
		public Action OffShift;
		public Action OnPrimary;
		public Action OffPrimary;
		public Action OnSecondary;
		public Action OffSecondary;
	}
}