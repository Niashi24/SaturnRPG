using UnityEngine;

namespace SaturnRPG.Battle
{
	[System.Serializable]
	public struct BattleStats
	{
		public int HP;
		public int MP;
		public int Attack;
		public int Magic;
		public int Defense;

		public static BattleStats operator +(BattleStats a, BattleStats b)
		{
			return new BattleStats()
			{
				HP = a.HP + b.HP,
				MP = a.MP + b.MP,
				Attack = a.Attack + b.Attack,
				Magic = a.Magic + b.Magic,
				Defense = a.Defense + b.Defense
			};
		}

		public static BattleStats operator *(BattleStats a, float m)
		{
			return new BattleStats()
			{
				HP = (int)Mathf.Round(a.HP * m),
				MP = (int)Mathf.Round(a.MP * m),
				Attack = (int)Mathf.Round(a.Attack * m),
				Defense = (int)Mathf.Round(a.Defense * m),
				Magic = (int)Mathf.Round(a.Magic * m),
			};
		}

		public BattleStats Multiply(float hp = 1, float mp = 1, float atk = 1, float def = 1, float mag = 1)
		{
			return new BattleStats()
			{
				HP = (int)Mathf.Round(this.HP * hp),
				MP = (int)Mathf.Round(this.MP * mp),
				Attack = (int)Mathf.Round(this.Attack * atk),
				Defense = (int)Mathf.Round(this.Defense * def),
				Magic = (int)Mathf.Round(this.Magic * mag),
			};
		}
	}
}