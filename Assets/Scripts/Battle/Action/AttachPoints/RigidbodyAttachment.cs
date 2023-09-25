using System;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public class RigidbodyAttachment : IAttachPoint
	{
		public Rigidbody2D AttachedRigidbody;
		public Transform AttachPoint;
		public bool ShouldBeMaxDistanceSwingOnly = true;
		public float MaxAccelerationPerFrame = 100f;
		
		public void OnAttach(Rigidbody2D user, DistanceJoint2D distanceJoint2D)
		{
			distanceJoint2D.connectedBody = AttachedRigidbody;
			distanceJoint2D.connectedAnchor = AttachedRigidbody.transform.InverseTransformPoint(AttachPoint.position);
		}

		public void Pull(Rigidbody2D user, float pullForce)
		{
			Vector2 position = AttachPoint.position;
			Vector2 pullAcceleration = (position - user.position).normalized * (pullForce * Time.deltaTime);
			user.velocity += pullAcceleration;
			AttachedRigidbody.AddForceAtPosition(-pullAcceleration, position, ForceMode2D.Impulse);
		}

		public void Swing(Rigidbody2D user, DistanceJoint2D distanceJoint2D)
		{
			distanceJoint2D.distance = Vector2.Distance(user.position, AttachPoint.position);
		}
	}
}	