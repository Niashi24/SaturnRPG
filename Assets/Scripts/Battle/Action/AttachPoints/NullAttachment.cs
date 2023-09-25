using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public class NullAttachment : IAttachPoint
	{
		/// <summary>
		/// A cached version of the NullAttachment since there's no reason to make multiple.
		/// </summary>
		public static readonly NullAttachment Null = new();

		public void OnAttach(Rigidbody2D user, DistanceJoint2D distanceJoint2D)
		{
			Debug.LogWarning("Tried to attach on a null attachment.");
		}

		public void Pull(Rigidbody2D user, float pullForce)
		{
			Debug.LogWarning("Tried to pull on a null attachment.");
		}

		public void Swing(Rigidbody2D user, DistanceJoint2D distanceJoint2D)
		{
			Debug.LogWarning("Tried to swing on a null attachment.");
		}
	}
}