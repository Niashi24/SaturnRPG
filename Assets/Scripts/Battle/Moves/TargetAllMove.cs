using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Battle.Moves
{
    public class TargetAllMove : BattleMoveComponent
    {
        public override async UniTask PlayAttack(BattleContext context, BattleAttack attack)
        {
            BattleStats userStats = attack.Stats;
            BattleStats enemyStats = attack.Target.GetBattleStats();
			
            Debug.Log($"{attack.User.Name} attacked {attack.Target.Name}!");
            await attack.User.UnitVisual.PartyMemberVisual.PlayAnimation("TestMoveAnim");

            int damage = userStats.Attack - enemyStats.Defense;
            damage = Math.Max(0, damage);

            await context.BattleCamera.SetTargetAndWait(attack.Target.Viewable3D);
			
            await attack.Target.DealDamage(damage);
        }

        public override List<ITargetable> GetTargetables(BattleUnit user, BattleContext context)
        {
            var targetables = new List<ITargetable>();
            targetables.AddRange(context.PlayerUnitManager.ActiveUnits);
            targetables.AddRange(context.EnemyUnitManager.ActiveUnits);
            targetables.Remove(user);
            return targetables;
        }
    }
}
