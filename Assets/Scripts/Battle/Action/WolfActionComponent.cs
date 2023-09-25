using System;
using System.Numerics;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace SaturnRPG.Battle.BattleAction
{
	public class WolfActionComponent : PlayerActionComponent
	{
		[Header("Static Physics")]
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

		[Header("Jump")]
		[SerializeField, Min(0.001f)]
		private float timeToJump = 0.5f;

		[SerializeField]
		private AnimationCurve jumpForceCurve;

		[Header("Bark")]
		[SerializeField, Min(0.001f)]
		private float timeToBark = 0.5f;

		[SerializeField]
		private AnimationCurve barkForceCurve;

		[SerializeField]
		private AnimationCurve barkRangeCurve;

		[SerializeField]
		private float barkMaxAngleDegrees = 60f;

		private bool _isGrounded = false;

		private bool _isChargingJump = false;
		private float _jumpTimer = 0;

		private bool _isChargingBark = false;
		private float _barkTimer = 0;

		public event Action<float> JumpTimerChanged, BarkTimerChanged;
		public event Action OnStartChargeJump, OnStartChargeBark;
		public event Action<float> OnJump;
		public event Action<float, float> OnBark;

		private void OnEnable()
		{
			MainInput.OnSecondary += StartChargeJump;
			MainInput.OffSecondary += ChargeJump;
			MainInput.OnPrimary += StartChargeBark;
			MainInput.OffPrimary += ChargeBark;
		}

		private void OnDisable()
		{
			MainInput.OnSecondary -= StartChargeJump;
			MainInput.OffSecondary -= ChargeJump;
			MainInput.OnPrimary -= StartChargeBark;
			MainInput.OffPrimary -= ChargeBark;
		}

		private void FixedUpdate()
		{
			_isGrounded = IsGrounded();

			Movement();

			ChargeJumping();
			ChargeBarking();
		}

		private void Movement()
		{
			var moveAcceleration = this.moveAcceleration;
			var maxGroundXSpeed = this.maxGroundXSpeed;
			if (!_isGrounded) moveAcceleration /= 4f;
			if (_isChargingJump) maxGroundXSpeed /= 2f;
			if (_isChargingBark) maxGroundXSpeed /= 2f;
			if (MainInput.Shift)
			{
				moveAcceleration *= 2f;
				maxGroundXSpeed *= 2f;
			}

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
		}

		private void UpdateJumpTimer(float time)
		{
			_jumpTimer = time;
			JumpTimerChanged?.Invoke(time / timeToJump);
		}

		private void UpdateBarkTimer(float time)
		{
			_barkTimer = time;
			BarkTimerChanged?.Invoke(time / timeToBark);
		}

		private void ChargeJumping()
		{
			if (!_isChargingJump) return;
			UpdateJumpTimer(Mathf.Min(timeToJump, _jumpTimer + Time.deltaTime));
		}

		private void StartChargeJump()
		{
			_isChargingJump = true;
			UpdateJumpTimer(0);
			OnStartChargeJump?.Invoke();
		}

		private void ChargeJump()
		{
			if (!_isChargingJump) return;

			if (IsGrounded())
			{
				var t = _jumpTimer / timeToJump;
				rbdy2D.velocity += MainInput.AimDirection * jumpForceCurve.Evaluate(_jumpTimer / timeToJump);
				OnJump?.Invoke(t);
			}

			_isChargingJump = false;
			UpdateJumpTimer(0);
		}

		private void ChargeBarking()
		{
			if (!_isChargingBark) return;
			UpdateBarkTimer(Mathf.Min(timeToBark, _barkTimer + Time.deltaTime));
		}

		private void StartChargeBark()
		{
			_isChargingBark = true;
			UpdateBarkTimer(0);
			OnStartChargeBark?.Invoke();
		}

		private void ChargeBark()
		{
			if (!_isChargingBark) return;


			var t = _barkTimer / timeToBark;

			float distance = barkRangeCurve.Evaluate(t);
			float force = barkForceCurve.Evaluate(t);

			(-barkMaxAngleDegrees).StepTo(barkMaxAngleDegrees, 10).ForEach(x =>
			{
				Debug.DrawRay(rbdy2D.position, MainInput.AimDirection.Rotate(x * Mathf.Deg2Rad) * distance,
					Color.green, 0.5f);
			});

			// Debug.DrawRay(rbdy2D.position,
			// 	MainInput.AimDirection.Rotate(barkMaxAngleDegrees * Mathf.Deg2Rad) * distance, Color.green, 0.5f);
			// Debug.DrawRay(rbdy2D.position,
			// 	MainInput.AimDirection.Rotate(-barkMaxAngleDegrees * Mathf.Deg2Rad) * distance, Color.green, 0.5f);
			float minDot = Mathf.Cos(barkMaxAngleDegrees * Mathf.Deg2Rad);
			var results = Physics2D.OverlapCircleAll(rbdy2D.position, distance, collisionMask);
			// int numObjects = 0;
			Vector2 totalForce = Vector2.zero;
			foreach (var coll in results)
			{
				var otherRbdy = coll.attachedRigidbody;
				if (otherRbdy == null || otherRbdy.bodyType == RigidbodyType2D.Static) continue;

				var direction = rbdy2D.position.DirectionTo(otherRbdy.position);
				float dotProd = Vector2.Dot(direction, MainInput.AimDirection);
				// Not within cone
				if (dotProd < minDot) continue;

				// numObjects++;

				Vector2 appliedForce = dotProd.Remap(minDot, 1, 0.5f, 1) * direction * force;

				otherRbdy.velocity += appliedForce;
				totalForce += appliedForce;
				// otherRbdy.velocity += force.ProjectToDirection(direction);
			}

			// Equal and opposite reaction :)
			rbdy2D.velocity -= totalForce;

			OnBark?.Invoke(t, distance);
			UpdateBarkTimer(0f);
			_isChargingBark = false;
		}

		private bool IsGrounded()
		{
			// multiply radius by 0.95f so you can't jump off walls
			return Physics2D.CircleCast(rbdy2D.position, coll2D.radius * 0.95f, Vector2.down, 1, collisionMask);
		}
	}
}