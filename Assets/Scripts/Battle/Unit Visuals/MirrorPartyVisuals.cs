using System;
using System.Collections;
using System.Collections.Generic;
using SaturnRPG.Battle;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace SaturnRPG
{
    public class MirrorPartyVisuals : MonoBehaviour
    {
        [SerializeField, Required]
        private BattleUnitManager battleUnitManager;

        private void OnEnable()
        {
            battleUnitManager.OnSetActiveUnits += MirrorUnits;
        }

        private void OnDisable()
        {
            battleUnitManager.OnSetActiveUnits -= MirrorUnits;
        }

        private void MirrorUnits(List<BattleUnit> units)
        {
            foreach (var unit in units)
            {
                unit.UnitVisual.PartyMemberVisual.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
