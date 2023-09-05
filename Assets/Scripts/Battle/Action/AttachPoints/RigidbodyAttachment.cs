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
		public event Action OnOverAccelerationPerFrame;

		public void Pull(Rigidbody2D user, float pullForce)
		{
			Vector2 position = AttachPoint.position;
			Vector2 pullAcceleration = (position - user.position).normalized * (pullForce * Time.deltaTime);
			user.velocity += pullAcceleration;
			AttachedRigidbody.velocity -= pullAcceleration;
			// user.AddForce(pullAcceleration);
			// AttachedRigidbody.AddForceAtPosition(-pullAcceleration, position);
		}

		public void Swing(Rigidbody2D user)
		{
			var relativeVelocity = user.velocity + AttachedRigidbody.velocity;
			var aC = IAttachPoint.GetCentripetalAcceleration(user.position, AttachPoint.position, relativeVelocity,
				ShouldBeMaxDistanceSwingOnly);

			var aCUser = IAttachPoint.GetCentripetalAcceleration(user.position, AttachPoint.position,
				user.velocity,
				ShouldBeMaxDistanceSwingOnly);

			var aCOther = IAttachPoint.GetCentripetalAcceleration(AttachPoint.position, user.position,
				AttachedRigidbody.velocity,
				ShouldBeMaxDistanceSwingOnly);

			if (aC.magnitude > MaxAccelerationPerFrame)
			{
				aC = aC.WithMagnitude(MaxAccelerationPerFrame);
				OnOverAccelerationPerFrame?.Invoke();
			}

			user.velocity += aCUser - aCOther;
			AttachedRigidbody.velocity += aCOther - aCUser;
		}
	}
}