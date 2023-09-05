using System;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
	public class SpiderWebShot : MonoBehaviour
	{
		[SerializeField]
		private LayerMask collisionMask;

		[SerializeField, Required]
		private Transform parentTransform;

		[SerializeField, Required]
		private Rigidbody2D spider;

		[SerializeField, Required]
		private LineRenderer lineRenderer;
		
		[SerializeField]
		private float moveSpeed = 120f;

		[SerializeField]
		private float radius = 1f;

		[ShowInInspector, ReadOnly]
		public State ShotState { get; private set; } = State.Inactive;
		
		[ShowInInspector, ReadOnly]
		private IAttachPoint _attachPoint = NullAttachment.Null;
		
		private Vector2 _shotDirection = Vector2.zero;

		private Transform _transform;

		private NullAttachment _nullAttachment = NullAttachment.Null;
		private StaticColliderAttachment _staticColliderAttachment;
		private RigidbodyAttachment _rigidbodyAttachment;

		private void Awake()
		{
			_transform = transform;
			_staticColliderAttachment = new StaticColliderAttachment()
			{
				AttachPoint = _transform
			};
			_rigidbodyAttachment = new RigidbodyAttachment()
			{
				AttachPoint = _transform
			};
		}

		public enum State
		{
			Inactive,
			InMotion,
			Attached
		}

		private void FixedUpdate()
		{
			lineRenderer.SetPosition(1, spider.position.ToVector3() - transform.position);
			
			if (ShotState != State.InMotion) return;

			float distance = Time.deltaTime * moveSpeed;

			var hit = Physics2D.CircleCast(transform.position, radius, _shotDirection, distance, collisionMask);

			if (hit)
			{
				ShotState = State.Attached;
				_transform.position = hit.point;
				_transform.parent = hit.transform;

				if (hit.rigidbody != null && hit.rigidbody.bodyType != RigidbodyType2D.Static)
				{
					_attachPoint = _rigidbodyAttachment;
					_rigidbodyAttachment.AttachedRigidbody = hit.rigidbody;
				}
				else
				{
					_attachPoint = _staticColliderAttachment;
				}
			}
			else
			{
				transform.position += (Vector3)(distance * _shotDirection);
			}
			
		}

		// private RaycastHit2D TryRaycast(LayerMask layerMask, float distance)
		// {
		// 	// var raycastHits = new RaycastHit2D[1];
		// 	return Physics2D.Raycast(transform.position, _shotDirection, distance, layerMask);
		// }

		public void StartShot(Vector2 startPosition, Vector2 direction)
		{
			gameObject.SetActive(true);
			ShotState = State.InMotion;
			_transform.position = startPosition;
			_shotDirection = direction;
		}

		public void StopShot()
		{
			gameObject.SetActive(false);
			ShotState = State.Inactive;
			_attachPoint = _nullAttachment;
			_transform.parent = parentTransform;
		}

		public void Pull(Rigidbody2D spiderRigidbody2D, float pullForce)
		{
			if (ShotState != State.Attached) return;
			_attachPoint.Pull(spiderRigidbody2D, pullForce);
		}

		public void Swing(Rigidbody2D spiderRigidbody2D)
		{
			if (ShotState != State.Attached) return;
			_attachPoint.Swing(spiderRigidbody2D);
		}
	}
}