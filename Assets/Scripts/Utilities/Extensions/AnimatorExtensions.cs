using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaturnRPG.Utilities.Extensions
{
	public static class AnimatorExtensions
	{
		public static async UniTask PlayAnimation(Animator animator, string animation)
		{
			animator.Play(animation);
			await UniTask.Delay((int)(1000 * (animator.GetCurrentAnimatorStateInfo(0).length +
			                                  animator.GetCurrentAnimatorStateInfo(0).normalizedTime)));
		}

		public static async UniTask PlayAnimation(Animator animator, string animation, CancellationToken cancellationToken)
		{
			animator.Play(animation);
			await UniTask.Delay((int)(1000 * (animator.GetCurrentAnimatorStateInfo(0).length +
			                           animator.GetCurrentAnimatorStateInfo(0).normalizedTime)),
				cancellationToken: cancellationToken);
		}
	}
}