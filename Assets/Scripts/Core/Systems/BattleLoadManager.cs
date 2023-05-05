using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using SaturnRPG.Core;
using SaturnRPG.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaturnRPG.Battle
{
    public class BattleLoadManager : MonoBehaviour
    {
        public readonly AsyncEvent<BattleEncounter> OnStartLoadBattle = new(),
            OnLoadBattle = new(),
            OnFinishLoadBattle = new();
        
        public readonly AsyncEvent<BattleEncounter> OnFinishBattle = new();

        public readonly AsyncEvent<BattleEncounter> OnStartUnloadBattle = new(),
            OnUnloadBattle = new(),
            OnFinishUnloadBattle = new();

        private void Start()
        {
            OnLoadBattle.Subscribe(LoadBackground);
            UniTask LoadBackground(BattleEncounter battleEncounter)
            {
                if (battleEncounter.Background.BackgroundPrefab != null)
                    Instantiate(battleEncounter.Background.BackgroundPrefab, Vector3.zero, Quaternion.identity);
                return UniTask.CompletedTask;
            }
        }

        [Button, DisableInEditorMode]
        public async UniTask PerformBattle(BattleParty activeParty, BattleEncounter battleEncounter)
        {
            await OnStartLoadBattle.Invoke(battleEncounter);

            var loadSceneTask = !SceneManager.GetSceneByName(Constants.BattleScene).isLoaded ?
                SceneManager.LoadSceneAsync(Constants.BattleScene, LoadSceneMode.Additive).ToUniTask()
                : UniTask.CompletedTask;
            var loadBattleTask = OnLoadBattle.Invoke(battleEncounter);
            await UniTask.WhenAll(loadSceneTask, loadBattleTask);

            await OnFinishLoadBattle.Invoke(battleEncounter);

            await BattleManager.I.StartBattle(activeParty, battleEncounter.EnemyParty);
            await OnFinishBattle.Invoke(battleEncounter);

            await OnStartUnloadBattle.Invoke(battleEncounter);

            // Have to use ? because may be unable to load if battle is base scene
            var unloadSceneTask = SceneManager.UnloadSceneAsync("BattleScene")?.ToUniTask() ?? UniTask.CompletedTask;
            var unloadBattleTask = OnUnloadBattle.Invoke(battleEncounter);
            await UniTask.WhenAll(unloadSceneTask, unloadBattleTask);

            await OnFinishUnloadBattle.Invoke(battleEncounter);
        }
    }
}
