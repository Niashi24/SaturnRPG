using UnityEngine;

namespace SaturnRPG.Battle
{
	public class ConstantViewable : I3DViewable
	{
		public Vector3 Position;

		public Vector3 GetPosition() => Position;
	}
}