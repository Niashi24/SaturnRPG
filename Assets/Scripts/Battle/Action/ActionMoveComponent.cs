using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.BattleAction
{
    public abstract class ActionMoveComponent : BattleMoveComponent
    {
        [SerializeField, Required]
        private Transform playerSpawnLocation;

        public override UniTask PlayAttack(BattleContext context, BattleAttack attack)
        {
            throw new System.NotImplementedException();
        }

        public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
