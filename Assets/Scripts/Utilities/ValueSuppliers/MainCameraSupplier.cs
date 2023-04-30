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
            get
            {
                // if (_mainCamera == null)
                //     _mainCamera = Camera.main;
                // return _mainCamera;
                return Systems.I.MainCameraManager.MainCamera;
            }
            set
            {
                Debug.LogWarning("Tried to set Main Camera");
            }
        }
    }
}
