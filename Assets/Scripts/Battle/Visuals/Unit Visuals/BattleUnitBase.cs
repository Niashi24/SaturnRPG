using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle.Visuals
{
	public class BattleUnitBase : MonoBehaviour
	{
		// [SerializeField, Required]
		// private SpriteRenderer renderer;
		//
		// // [Seriali]
		
		public void SetAnchorUnit(BattleUnit unit, Transform trans)
		{
			transform.SetParent(trans, false);
		}
	}
}