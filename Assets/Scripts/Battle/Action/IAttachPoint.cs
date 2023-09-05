using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public interface IAttachPoint
	{
		void Pull(Rigidbody2D user, float pullForce);

		void Swing(Rigidbody2D user);

		public const float MINIMUM_SWING_DISTANCE = 0.01f;

		public static Vector2 GetCentripetalAcceleration(
			Vector2 user, Vector2 attachment, Vector2 relativeVelocity,
			bool shouldBeMaxDistanceSwingOnly = false)
		{
			float r = Vector2.Distance(user, attachment);
			if (r < MINIMUM_SWING_DISTANCE) return Vector2.zero;

			var swingDirection = (attachment - user).normalized;
			float tangentVelSqr = relativeVelocity.sqrMagnitude -
			                      Mathf.Pow(Vector2.Dot(swingDirection, relativeVelocity), 2);
			// Get centripetal force/acceleration to keep object in uniform circular motion
			Vector2 aC = tangentVelSqr / r * Time.deltaTime * swingDirection;
			
			// Get Velocity that isn't going in the direction of the swing (normal to swing)
			float dotProd = Vector2.Dot(swingDirection, relativeVelocity);
			Vector2 normalVelocity = swingDirection * dotProd;
			// Remove normal velocity so it doesn't fall out of the swing
			if (shouldBeMaxDistanceSwingOnly)
			{
				if (dotProd < 0) // Moving away from attachment
				{
					aC -= normalVelocity;
				}
			}
			else
			{
				aC -= normalVelocity;
			}

			return aC;
		}
	}
}