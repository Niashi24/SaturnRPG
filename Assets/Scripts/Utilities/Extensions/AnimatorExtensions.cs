using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Utilities.Extensions
{
	public static class AnimatorExtensions
	{
		public static async UniTask PlayAnimation(this Animator animator, string animation)
		{
			var stateID = Animator.StringToHash(animation);
			if (!animator.HasState(0, Animator.StringToHash(animation))) return;
			animator.Play(stateID);
			
			await UniTask.Delay((int)(1000 * (animator.GetCurrentAnimatorStateInfo(0).length +
			                                  animator.GetCurrentAnimatorStateInfo(0).normalizedTime)));
		}

		public static async UniTask PlayAnimation(this Animator animator, string animation, CancellationToken cancellationToken)
		{
			var stateID = Animator.StringToHash(animation);
			if (!animator.HasState(0, Animator.StringToHash(animation))) return;
			animator.Play(stateID);

			await UniTask.Delay((int)(1000 * (animator.GetCurrentAnimatorStateInfo(0).length +
			                           animator.GetCurrentAnimatorStateInfo(0).normalizedTime)),
				cancellationToken: cancellationToken);
		}
	}
}