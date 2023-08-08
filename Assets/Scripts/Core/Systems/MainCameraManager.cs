using LS.Utilities;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Core.Systems
{
	public class MainCameraManager : MonoSingleton<MainCameraManager>, IValueSupplier<Camera>
	{
		[field: SerializeField, Required]
		public Camera MainCamera { get; private set; }

		public Camera Value
		{
			get => MainCamera;
			set => Debug.LogWarning("Tried to set value of Main Camera Manager");
		}
	}
}