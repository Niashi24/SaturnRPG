﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LS.Utilities;
using UnityEngine;

namespace SaturnRPG.Utilities
{
	[System.Serializable]
	public class AsyncEvent
	{
		[SerializeField]
		private HashSet<Func<UniTask>> callbacks;

		public void Subscribe(Func<UniTask> callback)
		{
			callbacks.Add(callback);
		}

		public void Unsubscribe(Func<UniTask> callback)
		{
			callbacks.Remove(callback);
		}

		public async UniTask Invoke()
		{
			var tasks = callbacks
				.Select(x => x.Invoke())
				.ToArray();

			await UniTask.WhenAll(tasks);
			
			callbacks.Clear();
		}
	}
	
	[System.Serializable]
	public class AsyncEvent<T>
	{
		[SerializeField]
		private HashSet<Func<T, UniTask>> callbacks = new();

		public void Subscribe(Func<T, UniTask> callback)
		{
			callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T, UniTask> callback)
		{
			callbacks.Remove(callback);
		}

		public async Task Invoke(T value)
		{
			var tasks = callbacks
				.Select(x => x.Invoke(value))
				.ToArray();

			await UniTask.WhenAll(tasks);
			
			callbacks.Clear();
		}
	}
	
	[System.Serializable]
	public class AsyncEvent<T1, T2>
	{
		[SerializeField]
		private HashSet<Func<T1, T2, UniTask>> callbacks = new();

		public void Subscribe(Func<T1, T2, UniTask> callback)
		{
			callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T1, T2, UniTask> callback)
		{
			callbacks.Remove(callback);
		}

		public async Task Invoke(T1 value1, T2 value2)
		{
			var tasks = callbacks
				.Select(x => x.Invoke(value1, value2))
				.ToArray();

			await UniTask.WhenAll(tasks);
			
			callbacks.Clear();
		}
	}
}