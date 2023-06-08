using System.Collections.Generic;
using SaturnRPG.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
    public class OnTargetSelectionPanCamera : MonoBehaviour
    {
        [SerializeField, Required]
        private BattleCamera battleCamera;

        [SerializeField, Required]
        private BattleTargetChooserUI targetChooser;

        [SerializeField, Required]
        private MouseCameraControl mouseCameraControl;

        private void OnEnable()
        {
            targetChooser.OnStartSelection += SetMouseControl;
            targetChooser.OnEndSelection += ResetCameraControl;
        }

        private void OnDisable()
        {
            targetChooser.OnStartSelection -= SetMouseControl;
            targetChooser.OnEndSelection -= ResetCameraControl;
        }

        private void SetMouseControl(List<UITarget> targets)
        {
            mouseCameraControl.LoadTargets(targets);
            battleCamera.SetTarget(mouseCameraControl);
        }

        private void ResetCameraControl()
        {
            battleCamera.ClearTarget();
        }
    }
}
