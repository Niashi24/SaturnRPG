using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SaturnRPG
{
    public class BattleText : MonoBehaviour
    {
        [SerializeField, Required]
        private Text textField;

        public void SetText(string text)
        {
            textField.text = text;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetTextAndActive(string text, bool active = true)
        {
            SetText(text);
            SetActive(active);
        }
    }
}
