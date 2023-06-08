using LS.Utilities;
using SaturnRPG.Battle;
using UnityEngine;

namespace SaturnRPG.Camera3D2D
{
	[System.Serializable]
	public class ObjectReferenceViewable : I3DViewable
	{
		[SerializeField]
		private ObjectReference<I3DViewable> viewable3D;

		public ObjectReferenceViewable(Object size)
		{
			viewable3D = new ObjectReference<I3DViewable>(size as I3DViewable);
		}

		public Vector3 GetPosition() => viewable3D.HasValue ? viewable3D.Value.GetPosition() : Vector3.zero;
	}
}