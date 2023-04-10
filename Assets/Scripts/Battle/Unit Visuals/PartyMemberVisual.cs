using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SaturnRPG.Camera3D2D;
using SaturnRPG.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG
{
    public class PartyMemberVisual : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [field: SerializeReference, Required]
        public ISize Size { get; private set; } = new ManualSize();

        public async UniTask PlayAnimation(string animation)
        {
            if (animator == null) return;
            
            await animator.PlayAnimation(animation);
        }

        public async UniTask PlayAnimation(string animation, CancellationToken token)
        {
            if (animator == null) return;
            
            await animator.PlayAnimation(animation, token);
        }
    }
}
