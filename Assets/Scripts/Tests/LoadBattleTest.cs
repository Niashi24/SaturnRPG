using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaturnRPG
{
    public class LoadBattleTest : MonoBehaviour
    {
        public readonly AsyncEvent OnStartLoadBattle = new(), OnFinishLoadBattle = new();
        public readonly AsyncEvent OnFinishBattle = new();

        [Button, DisableInEditorMode]
        public async UniTask PerformBattle(BattleParty activeParty, BattleEncounter battleEncounter)
        {
            await OnStartLoadBattle.Invoke();
            await SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
            await OnFinishLoadBattle.Invoke();
            
            if (battleEncounter.BackgroundPrefab != null)
                Instantiate(battleEncounter.BackgroundPrefab, Vector3.zero, Quaternion.identity);
            
            await BattleManager.I.StartBattle(activeParty, battleEncounter.EnemyParty);

            await OnFinishBattle.Invoke();
            await SceneManager.UnloadSceneAsync("BattleScene");
        }
    }
}
