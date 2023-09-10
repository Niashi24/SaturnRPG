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

		private bool isGrounded = false;

		private void OnEnable()
		{
			mainInput.OnSecondary += StartShoot;
			mainInput.OffSecondary += StopShoot;
		}

		private void OnDisable()
		{
			mainInput.OnSecondary -= StartShoot;
			mainInput.OffSecondary -= StopShoot;
		}

		private void FixedUpdate()
		{
			isGrounded = IsGrounded();

			// if (mainInput.MoveDirection != Vector2.zero && isGrounded)
			// 	Debug.Log("here");

			var moveAcceleration = this.moveAcceleration;
			if (!isGrounded) moveAcceleration /= 4f;

			if (true)
			{
				float xDir = mainInput.MoveDirection.x;
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
				else if (isGrounded || xDir != 0) //if (xDir != 0)
				{
					rbdy2D.velocity = rbdy2D.velocity.With(x: xVel.IncreaseAbs(-moveAcceleration * Time.deltaTime));
				}

				// rbdy2D.AddForce(mainInput.MoveDirection.With(y: 0) * (moveAcceleration * Time.deltaTime));
			}

			WebSwinging();
		}

		private void WebSwinging()
		{
			if (spiderWebShot.ShotState != SpiderWebShot.State.Attached) return;

			if (mainInput.Shift)
				spiderWebShot.Swing(rbdy2D);
			else if (mainInput.Primary)
				spiderWebShot.Pull(rbdy2D, pullForce);
		}

		private bool IsGrounded()
		{
			return Physics2D.CircleCast(rbdy2D.position, coll2D.radius, Vector2.down, 1, collisionMask);
		}

		private void StartShoot()
		{
			if (mainInput.AimDirection == Vector2.zero) return;
			spiderWebShot.StartShot(rbdy2D.position, mainInput.AimDirection);
		}

		private void StopShoot()
		{
			spiderWebShot.StopShot();
		}
	}
}