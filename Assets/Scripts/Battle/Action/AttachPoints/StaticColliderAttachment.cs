using System;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public class StaticColliderAttachment : IAttachPoint
	{
		public Transform AttachPoint;
		public bool ShouldBeMaxDistanceSwingOnly = true;
		public float MaxAccelerationPerFrame = 1000f;

		public void OnAttach(Rigidbody2D user, DistanceJoint2D distanceJoint2D)
		{
			distanceJoint2D.connectedBody = null;
			distanceJoint2D.connectedAnchor = AttachPoint.position;
		}

		public void Pull(Rigidbody2D user, float pullForce)
		{
			user.velocity += user.position.DirectionTo(AttachPoint.position) * (pullForce * Time.deltaTime);
		}

		public void Swing(Rigidbody2D user, DistanceJoint2D distanceJoint2D)
		{
			distanceJoint2D.distance = Vector2.Distance(user.position, AttachPoint.position);
			// distanceJoint2D.
			// var aC = IAttachPoint.GetCentripetalAcceleration(user.position, AttachPoint.position, user.velocity,
			// 	ShouldBeMaxDistanceSwingOnly);
			//
			// if (aC.magnitude > MaxAccelerationPerFrame)
			// {
			// 	aC = aC.WithMagnitude(MaxAccelerationPerFrame);
			// 	OnOverAccelerationPerFrame?.Invoke();
			// }
			//
			// user.velocity += aC;
		}
	}
}