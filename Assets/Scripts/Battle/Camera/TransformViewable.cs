using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	[System.Serializable]
	public class TransformViewable : I3DViewable
	{
		[SerializeField]
		public Transform Transform;

		public Vector3 GetPosition()
		{
			return Transform == null ? Vector3.zero : Transform.position;
		}
	}
}