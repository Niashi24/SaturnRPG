using System;
using System.Collections;
using System.Collections.Generic;
using SaturnRPG.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG
{
    public class TestHealthbar : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private int hp = 100;
        [SerializeField, Min(0)]
        private int maxHP = 100;

        [SerializeField, Required]
        private DelayValueBar valueBar;

        [Button]
        private void Damage()
        {
            hp = Math.Clamp(hp - 10, 0, maxHP);
            UpdateBar();
        }

        [Button]
        private void Heal()
        {
            hp = Math.Clamp(hp + 10, 0, maxHP);
            UpdateBar();
        }

        private void UpdateBar()
        {
            if (maxHP == 0) return;
            
            valueBar.SetValue((float)hp / maxHP);
        }
    }
}
