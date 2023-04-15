using LS.SearchWindows;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaturnRPG.Battle
{
	[CreateAssetMenu(fileName = "New Battle Encounter", menuName = "Battle/Encounter")]
	public class BattleEncounter : ScriptableObject
	{
		[field: SerializeField, Required]
		public BattleParty EnemyParty { get; private set; }
		
		[field: SerializeField, AssetSearch]
		public GameObject BackgroundPrefab { get; private set; }
	}
}