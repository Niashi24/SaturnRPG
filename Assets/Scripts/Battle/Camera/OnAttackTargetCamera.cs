using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG
{
    public class OnAttackTargetCamera : MonoBehaviour
    {
	    [SerializeField, Required]
	    private BattleCamera battleCamera;

	    [SerializeField, Required]
	    private BattleAttackManager attackManager;

	    void OnEnable()
	    {
			attackManager.OnAttack.Subscribe(TargetAttackUser);
	    }

	    void OnDisable()
	    {
		    attackManager.OnAttack.Unsubscribe(TargetAttackUser);
	    }

	    private async UniTask TargetAttackUser(BattleAttack attack, BattleContext context)
	    {
			await battleCamera.SetTargetAndWait(attack.User.UnitVisual);
	    }
    }
}
