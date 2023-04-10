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
		public PartyMemberVisual PartyMemberVisual { get; private set; }

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
			if (partyMember.VisualPrefab == null)
			{
				// Add blank animator
				var obj = new GameObject("Unit Visual Prefab");
				obj.transform.parent = transform;
				PartyMemberVisual = obj.AddComponent<PartyMemberVisual>();
				
				Debug.LogWarning($"Party Member {partyMember.Name} is missing an Visual Component.");
				return;
			}

			PartyMemberVisual = Instantiate(partyMember.VisualPrefab, Vector3.zero, Quaternion.identity, transform);
			PartyMemberVisual.transform.localPosition = Vector3.zero;
			
			cameraAnchor.SetSize(PartyMemberVisual.Size);
		}

		private UniTask CleanUpOnEnd(BattleState state)
		{
			if (state == BattleState.End)
				CleanUpVisuals();

			return UniTask.CompletedTask;
		}

		private void CleanUpVisuals()
		{
			Destroy(PartyMemberVisual.gameObject);
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