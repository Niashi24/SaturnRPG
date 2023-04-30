using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaturnRPG.Core.Systems
{
	public static class Bootstrapper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Execute()
		{
			// var scene = SceneManager.GetSceneByName("Systems");
			// Debug.Log($"IsSubScene: {scene.isSubScene}, IsLoaded: {scene.isLoaded}");
			// if (scene is { isSubScene: true, isLoaded: false })
			// 	SceneManager.LoadScene("Systems", LoadSceneMode.Additive);
			if (Systems.Loaded) return;

			Systems.Load();
		}
	}
}