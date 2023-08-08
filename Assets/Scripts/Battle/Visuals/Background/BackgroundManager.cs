using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Core;
using SaturnRPG.Core.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG.Battle
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField, Required]
        private Image wallImage;

        [SerializeField, Required]
        private MeshRenderer[] floorTiles;

        private void OnEnable()
        {
            BattleLoadManager.OnLoadBattle.Subscribe(LoadBackground);
        }

        private void OnDisable()
        {
            BattleLoadManager.OnLoadBattle.Unsubscribe(LoadBackground);
        }

        private UniTask LoadBackground(BattleEncounter encounter)
        {
            var background = encounter.Background;

            wallImage.sprite = background.Wall;

            foreach (var renderer in floorTiles)
            {
                renderer.material.SetTexture(Constants.MainTex, background.Floor);
            }

            if (background.BackgroundPrefab != null)
                Instantiate(background.BackgroundPrefab, Vector3.zero, Quaternion.identity, transform);            
            return UniTask.CompletedTask;
        }
    }
}
