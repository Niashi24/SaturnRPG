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
			
			transform.localPosition = Vector3.SmoothDamp(
				transform.localPosition,
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
	}
}