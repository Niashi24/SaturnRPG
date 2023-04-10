using Cysharp.Threading.Tasks;
using SaturnRPG.Camera3D2D;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class UnitVisual : MonoBehaviour, I3DViewable
	{
		[SerializeField, Required]
		private BattleUnit battleUnit;
		
		[field: SerializeField, LabelText("3D View Transform")]
		public Transform Anchor3D { get; private set; }

		[SerializeField, Required]
		private CameraAnchorTransformation cameraAnchor;

		[ShowInInspector, ReadOnly]
		public Animator Animator { get; private set; }

		private void OnEnable()
		{
			battleUnit.OnSetPartyMember += SetPartyMember;
		}
		
		private void Start() { BattleManager.I.OnBattleStateChange.Subscribe(CleanUpOnEnd); }

		private void OnDisable()
		{
			battleUnit.OnSetPartyMember -= SetPartyMember;
		}

		private void SetPartyMember(PartyMember partyMember)
		{
			if (partyMember.AnimatorPrefab == null)
			{
				// Add blank animator
				var obj = Instantiate(new GameObject("Animator"), Vector3.zero, Quaternion.identity, transform);
				Animator = obj.AddComponent<Animator>();
				
				Debug.LogWarning($"Party Member {partyMember.Name} is missing an animator.");
				return;
			}

			Animator = Instantiate(partyMember.AnimatorPrefab, Vector3.zero, Quaternion.identity, transform);
		}

		private UniTask CleanUpOnEnd(BattleState state)
		{
			if (state == BattleState.End)
				CleanUpVisuals();

			return UniTask.CompletedTask;
		}

		private void CleanUpVisuals()
		{
			Destroy(Animator.gameObject);
		}

		public Vector3 GetPosition()
		{
			if (Anchor3D != null)
				return Anchor3D.position;
			return Vector3.zero;
		}

		public void SetAnchor(Transform anchor3D)
		{
			Anchor3D = anchor3D;
			cameraAnchor.SetAnchor(Anchor3D);
		}
	}
}