using System.Threading;
using Cysharp.Threading.Tasks;
using LS.Utilities;
using SaturnRPG.UI;
using SaturnRPG.UI.HealthBar;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.Unit_Visuals
{
    public class DisplayHealthBarOnHit : MonoBehaviour
    {
        [SerializeField, Required]
        private BattleUnit unit;

        [SerializeField, Required]
        private ObjectReference<IValueBar> valueBar;

        private void OnEnable()
        {
            unit.OnHPChange.Subscribe(DisplayHealthBar);
            unit.OnSetPartyMember += UpdateHPImmediate;
            valueBar.Value?.SetActive(false);
        }

        private void OnDisable()
        {
            unit.OnHPChange.Unsubscribe(DisplayHealthBar);
            unit.OnSetPartyMember -= UpdateHPImmediate;
            valueBar.Value?.SetActive(false);
        }

        private void UpdateHPImmediate(PartyMember partyMember)
        {
            int maxHP = unit.GetBattleStats().HP;
            int hp = unit.HP;
            
            valueBar.Value?.SetValueImmediate((float)hp / maxHP);
        }

        private async UniTask DisplayHealthBar(int hp, int oldHP)
        {
            valueBar.Value?.SetActive(true);
            int maxHP = unit.GetBattleStats().HP;

            if (maxHP == 0) return;
            
            await valueBar.Value.SetValueAsync((float)hp / maxHP, BattleManager.I.BattleContext.BattleCancellationToken);
            valueBar.Value?.SetActive(false);
        }
    }
}