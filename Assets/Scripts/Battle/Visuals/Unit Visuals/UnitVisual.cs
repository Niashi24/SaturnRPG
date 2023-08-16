using System;
using Cysharp.Threading.Tasks;
using LS.Utilities;
using SaturnRPG.Battle.Unit_Visuals;
using SaturnRPG.Camera3D2D;
using SaturnRPG.UI.HealthBar;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	public class UnitVisual : MonoBehaviour, I3DViewable
	{
		[SerializeField, Required]
		private BattleUnit battleUnit;
		
		[ShowInInspector, ReadOnly, LabelText("3D View Transform")]
		public Transform Anchor3D { get; private set; }

		[SerializeField, Required]
		private CameraAnchorTransformation cameraAnchor;
		
		[field: SerializeField, Required]
		public ObjectReference<IValueBar> HealthBar { get; private set; }

		[ShowInInspector, ReadOnly]
		public PartyMemberVisual PartyMemberVisual { get; private set; }

		public event Action<PartyMemberVisual> OnSetPartyMemberVisual;
		public event Action<Transform> OnSetAnchor; 

		private void OnEnable()
		{
			battleUnit.OnSetPartyMember += SetPartyMember;
			battleUnit.OnHPChange.Subscribe(TakeDamage);
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
			PartyMemberVisual.Initialize(battleUnit, this);
			
			OnSetPartyMemberVisual?.Invoke(PartyMemberVisual);
			
			cameraAnchor.SetSize(PartyMemberVisual.Size);
		}

		private async UniTask TakeDamage(int HP, int oldHP)
		{
			var cancelToken = BattleManager.I.BattleContext.BattleCancellationToken;

			switch (HP)
			{
				case 0 when oldHP == 0:
					return;
				case 0 when oldHP > 0:
					await PartyMemberVisual.PlayAnimation(BattleVisualStates.MortalBlow, cancelToken);
					break;
				case > 0 when oldHP == 0:
					await PartyMemberVisual.PlayAnimation(BattleVisualStates.Revived, cancelToken);
					return;
				case > 0 when oldHP > 0:
					await PartyMemberVisual.PlayAnimation(BattleVisualStates.Hit, cancelToken);
					break;
			}
		}

		private UniTask CleanUpOnEnd(BattleState state)
		{
			if (state == BattleState.End)
				CleanUpVisuals();

			return UniTask.CompletedTask;
		}

		private void CleanUpVisuals()
		{
			if (PartyMemberVisual == null) return;
			
			Destroy(PartyMemberVisual.gameObject);
			cameraAnchor.SetSize(new ManualSize());
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
			cameraAnchor.Set3DViewable(Anchor3D.To3DViewable());
			OnSetAnchor?.Invoke(Anchor3D);
		}
	}
}