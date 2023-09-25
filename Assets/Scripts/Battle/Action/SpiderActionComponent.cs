using System;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.Battle.BattleAction
{
	public class SpiderActionComponent : PlayerActionComponent
	{
		[Header("Static Physics")]
		[SerializeField, Required]
		private SpiderWebShot spiderWebShot;

		[SerializeField, Required]
		private DistanceJoint2D distanceJoint2D;

		[SerializeField, Required]
		private Rigidbody2D rbdy2D;

		[SerializeField, Required]
		private CircleCollider2D coll2D;

		[SerializeField]
		private LayerMask playerMask;

		[SerializeField]
		private LayerMask collisionMask;

		[Header("Variables")]
		[SerializeField, Min(0)]
		private float moveAcceleration = 320f;

		[SerializeField, Min(0)]
		private float maxGroundXSpeed = 32f;

		[SerializeField]
		private float pullForce = 320f;

		private bool _isGrounded = false;

		private void OnEnable()
		{
			MainInput.OnSecondary += StartShoot;
			MainInput.OffSecondary += StopShoot;
		}

		private void OnDisable()
		{
			MainInput.OnSecondary -= StartShoot;
			MainInput.OffSecondary -= StopShoot;
		}

		private void FixedUpdate()
		{
			_isGrounded = IsGrounded();

			// if (mainInput.MoveDirection != Vector2.zero && isGrounded)
			// 	Debug.Log("here");

			var moveAcceleration = this.moveAcceleration;
			if (!_isGrounded) moveAcceleration /= 4f;

			if (true)
			{
				float xDir = MainInput.MoveDirection.x;
				var xVel = rbdy2D.velocity.x;
				if (xVel == 0)
				{
					rbdy2D.velocity =
						rbdy2D.velocity.With(x: (xDir * moveAcceleration * Time.deltaTime).MaxAbs(maxGroundXSpeed));
				}
				else if (Math.Sign(xDir) == Math.Sign(xVel))
				{
					if (Mathf.Abs(xVel) < maxGroundXSpeed)
					{
						rbdy2D.velocity =
							rbdy2D.velocity.With(
								x: xVel.IncreaseAbs(moveAcceleration * Time.deltaTime, maxGroundXSpeed));
					}
				}
				else if (_isGrounded || xDir != 0) //if (xDir != 0)
				{
					rbdy2D.velocity = rbdy2D.velocity.With(x: xVel.IncreaseAbs(-moveAcceleration * Time.deltaTime));
				}

				// rbdy2D.AddForce(mainInput.MoveDirection.With(y: 0) * (moveAcceleration * Time.deltaTime));
			}

			distanceJoint2D.enabled = MainInput.Shift && spiderWebShot.ShotState == SpiderWebShot.State.Attached;

			WebSwinging();
		}

		private void WebSwinging()
		{
			if (spiderWebShot.ShotState != SpiderWebShot.State.Attached) return;

			if (MainInput.Shift)
				spiderWebShot.Swing(rbdy2D);
			else if (MainInput.Primary)
				spiderWebShot.Pull(rbdy2D, pullForce);
		}

		private bool IsGrounded()
		{
			return Physics2D.CircleCast(rbdy2D.position, coll2D.radius, Vector2.down, 1, collisionMask);
		}

		private void StartShoot()
		{
			if (MainInput.AimDirection == Vector2.zero) return;
			spiderWebShot.StartShot(rbdy2D.position, MainInput.AimDirection);
		}

		private void StopShoot()
		{
			spiderWebShot.StopShot();
		}
	}
}