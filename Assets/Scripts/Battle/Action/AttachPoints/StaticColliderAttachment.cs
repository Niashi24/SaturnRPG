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
		public event Action OnOverAccelerationPerFrame;

		public void Pull(Rigidbody2D user, float pullForce)
		{
			user.velocity += user.position.DirectionTo(AttachPoint.position) * (pullForce * Time.deltaTime);
		}

		public void Swing(Rigidbody2D user)
		{
			var aC = IAttachPoint.GetCentripetalAcceleration(user.position, AttachPoint.position, user.velocity,
				ShouldBeMaxDistanceSwingOnly);

			if (aC.magnitude > MaxAccelerationPerFrame)
			{
				aC = aC.WithMagnitude(MaxAccelerationPerFrame);
				OnOverAccelerationPerFrame?.Invoke();
			}

			user.velocity += aC;
		}
	}
}