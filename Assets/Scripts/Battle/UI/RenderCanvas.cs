using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using SaturnRPG.Core.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG
{
    public class RenderCanvas : MonoBehaviour
    {
        [SerializeField, Required]
        private RawImage renderImage;

        private void OnEnable()
        {
            BattleLoadManager.OnLoadBattle.Subscribe(ShowRenderTexture);
            BattleLoadManager.OnUnloadBattle.Subscribe(HideRenderTexture);
        }

        private void OnDisable()
        {
            BattleLoadManager.OnLoadBattle.Unsubscribe(ShowRenderTexture);
            BattleLoadManager.OnUnloadBattle.Unsubscribe(HideRenderTexture);
        }

        private void HideRenderTexture()
        {
            renderImage.enabled = false;
        }

        private void ShowRenderTexture()
        {
            renderImage.enabled = true;
        }

        private UniTask HideRenderTexture(BattleEncounter encounter)
        {
            HideRenderTexture();
            return UniTask.CompletedTask;
        }

        private UniTask ShowRenderTexture(BattleEncounter encounter)
        {
            ShowRenderTexture();
            return UniTask.CompletedTask;
        }
    }
}
