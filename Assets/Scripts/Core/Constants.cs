using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaturnRPG.Core
{
	public static class Constants
	{
		public static readonly int MainTex = Shader.PropertyToID("_MainTex");

		public const string BattleSceneName = "BattleScene";

		public static readonly int BattleScene = SceneManager.GetSceneByName(BattleSceneName).buildIndex;
	}
}