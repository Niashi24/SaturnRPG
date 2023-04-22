using System.Threading;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.UI.HealthBar
{
	public interface IValueBar
	{
		void SetValueImmediate(float value);
		UniTask SetValueAsync(float value, CancellationToken cancellationToken);
		void SetActive(bool active);
	}
}