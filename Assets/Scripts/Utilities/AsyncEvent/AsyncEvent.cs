
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Utilities
{
	[Serializable]
	public class AsyncEvent
	{
		private HashSet<Func<UniTask>> _callbacks = new();

		public void Subscribe(Func<UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke()
		{
			var tasks = _callbacks
				.Select(x => x.Invoke())
				.ToArray();

			await UniTask.WhenAll(tasks);
			
			_callbacks.Clear();
		}
	}
	
	[Serializable]
	public class AsyncEvent<T>
	{
		private HashSet<Func<T, UniTask>> _callbacks = new();

		public void Subscribe(Func<T, UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T, UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke(T value)
		{
			var tasks = _callbacks
				.Select(x => x.Invoke(value))
				.ToArray();

			await UniTask.WhenAll(tasks);
			
			_callbacks.Clear();
		}
	}
	
	[Serializable]
	public class AsyncEvent<T1, T2>
	{
		private HashSet<Func<T1, T2, UniTask>> _callbacks = new();

		public void Subscribe(Func<T1, T2, UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T1, T2, UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke(T1 value1, T2 value2)
		{
			var tasks = _callbacks
				.Select(x => x.Invoke(value1, value2))
				.ToArray();

			await UniTask.WhenAll(tasks);
			
			_callbacks.Clear();
		}
	}
}