using System;
using SaturnRPG.Battle;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaturnRPG.Core.Systems
{
	public class Systems : MonoBehaviour
	{
		[field: SerializeField, Required]
		public BattleLoadManager BattleLoadManager { get; private set; }
		
		[field: SerializeField, Required]
		public MainCameraManager MainCameraManager { get; private set; }
		
		private static Systems _instance;
		public static Systems I => Load();

		public static Systems Load()
		{
			if (_instance != null) return _instance;
			
			#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && Application.isPlaying)
				return _instance;
			#endif
			
			_instance = FindObjectOfType<Systems>(true);
			if (_instance == null)
			{
				Debug.Log("Spawning system");
				_instance = Instantiate(Resources.Load("Systems")).GetComponent<Systems>();
			}
			
			if (_instance != null)
				DontDestroyOnLoad(_instance.gameObject);

			return _instance;
		}

		public static bool Loaded => _instance != null;

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			else if (_instance != this)
			{
				Debug.LogWarning("More than one Systems. Destroying duplicate.");
				Destroy(gameObject);
			}
		}

		private void OnDestroy()
		{
			// if (_instance == this)
			// 	_instance = null;
		}
	}
}