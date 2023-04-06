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

		[SerializeField]
		private float smoothTime = 0.3f;

		[ShowInInspector, ReadOnly]
		private Vector3 _velocity = Vector3.zero;

		[ShowInInspector, ReadOnly]
		private Vector3 _targetPosition;

		void Start()
		{
			_targetPosition = transform.position;
		}

		void Update()
		{
			transform.localPosition = Vector3.SmoothDamp(
				transform.localPosition,
				_targetPosition,
				ref _velocity,
				smoothTime
			);
		}

		[Button]
		[DisableInEditorMode]
		public void SetTargetPosition(Vector3 position)
		{
			position += targetOffset;
			_targetPosition = new Vector3(
				Mathf.Clamp(position.x, minMaxX.x, minMaxX.y),
				Mathf.Clamp(position.y, minMaxY.x, minMaxY.y),
				Mathf.Clamp(position.z, minMaxZ.x, minMaxZ.y)
			);
		}

		public void SetTarget(I3DViewable viewable)
		{
			SetTargetPosition(viewable.GetPosition());
		}
		
		#if UNITY_EDITOR
		public void SetTarget(LS.Utilities.ObjectReference<I3DViewable> viewable)
		{
			if (!viewable.HasValue) return;
			SetTarget(viewable.Value);
		}
		#endif
	}
}