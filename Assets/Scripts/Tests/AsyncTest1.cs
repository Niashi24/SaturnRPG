using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG
{
    public class AsyncTest1 : MonoBehaviour
    {
        [SerializeField] [Required] private BattleManager battleManager;

        [SerializeField] private Vector3 displacement;

        void Awake()
        {
            battleManager.OnBattleStart.Subscribe(Test);
        }

        async UniTask Test(BattleContext context)
        {
            Vector2 target = transform.position + displacement;
            while (Vector2.Distance(transform.position, target) > 0)
            {
                await UniTask.Yield();
                transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime);
            }
        }
    }
}
