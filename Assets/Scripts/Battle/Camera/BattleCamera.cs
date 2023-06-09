using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaturnRPG.Battle
{
	public class BattleCamera : MonoBehaviour
	{
		[Header("Movement Restrictions")]

		[SerializeField]
		private Vector2 minMaxX;
		[SerializeField]
		private Vector2 minMaxY;
		[SerializeField]
		private Vector2 minMaxZ;
		
		[SerializeField]
		private Vector3 targetOffset;

		[field: SerializeField]
		public float SmoothTime { get; private set; } = 0.3f;

		[ShowInInspector, ReadOnly]
		private Vector3 _velocity = Vector3.zero;

		[ShowInInspector, ReadOnly]
		private I3DViewable _target;

		private readonly ConstantViewable _constantViewable = new();

		private void Start()
		{
			_constantViewable.Position = transform.position;
			_target = _constantViewable;
		}

		private void Update()
		{
			var targetPosition = CalcTargetPosition(_target.GetPosition());
			
			// Debug.Log($"Target: {targetPosition}");
			
			transform.position = Vector3.SmoothDamp(
				transform.position,
				targetPosition,
				ref _velocity,
				SmoothTime
			);
		}

		[Button]
		[DisableInEditorMode]
		public void SetTargetPosition(Vector3 position)
		{
			_constantViewable.Position = position;
			_target = _constantViewable;
		}

		private Vector3 CalcTargetPosition(Vector3 rawPosition)
		{
			rawPosition += targetOffset;
			rawPosition = new Vector3(
				Mathf.Clamp(rawPosition.x, minMaxX.x, minMaxX.y),
				Mathf.Clamp(rawPosition.y, minMaxY.x, minMaxY.y),
				Mathf.Clamp(rawPosition.z, minMaxZ.x, minMaxZ.y)
			);
			return rawPosition;
		}

		[Button, DisableInEditorMode]
		public void SetTarget(I3DViewable viewable, bool follow = true)
		{
			if (follow)
			{
				_target = viewable;
			}
			else
			{
				_target = _constantViewable;
				_constantViewable.Position = _target.GetPosition();
			}
		}
		
		public void SetTarget(ITargetable targetable, bool follow = true)
		{
			if (targetable != null)
				SetTarget(targetable.Viewable3D, follow);
		}

		[Button]
		public void ClearTarget(bool returnToCenter = false)
		{
			_constantViewable.Position = returnToCenter ? Vector3.zero : _target.GetPosition();
			_target = _constantViewable;
		}

		public async UniTask SetTargetAndWait(I3DViewable viewable, bool follow = true)
		{
			var cancelToken = BattleManager.I.BattleContext.BattleCancellationToken;
			
			SetTarget(viewable);

			// const float minDX = 0.1f;
			//
			// float t = Time.time;
			//
			// while (Mathf.Abs((CalcTargetPosition(viewable.GetPosition()) - transform.position).magnitude) > minDX)
			// 	await UniTask.Yield(cancelToken);
			//
			// float dT = Time.time - t;
			//
			// Debug.Log($"Time taken: {dT}. Off by factor of {dT / SmoothTime}");

			await UniTask.Delay((int)(1000 * SmoothTime * 4));
			// Debug.Log((CalcTargetPosition(viewable.GetPosition()) - transform.position).magnitude);
		}

		public UniTask SetTargetAndWait(ITargetable targetable, bool follow = true)
		{
			if (targetable == null) return UniTask.CompletedTask;
			return SetTargetAndWait(targetable.Viewable3D, follow);
		}
	}
}