using System.Collections;
using System.Collections.Generic;
using LS.Utilities;
using SaturnRPG.Core.Systems;
using UnityEngine;

namespace SaturnRPG
{
    [CreateAssetMenu(menuName = "Variables/Camera/Main Camera")]
    public class MainCameraSupplier : ScriptableObject, IValueSupplier<Camera>
    {
        // private Camera _mainCamera;

        public Camera Value
        {
            get => Systems.Loaded ? Systems.I.MainCameraManager.MainCamera : Camera.main;
            set
            {
                Debug.LogWarning("Tried to set Main Camera");
            }
        }
    }
}
