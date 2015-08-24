using UnityEngine;
using System.Collections;

namespace Game {
	/// <summary>
	/// Bullet type.
	/// </summary>
	public enum BulletType {
		/// <summary>
		/// The normal.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// The heal.
		/// </summary>
		Heal = 1
	}

	/// <summary>
	/// Buff type.
	/// </summary>
	public enum BuffType {
		/// <summary>
		/// The none.
		/// </summary>
		None = 0,
		/// <summary>
		/// yicixingjiaxue
		/// </summary>
		OneUpHP = 1,
		/// <summary>
		/// chixuhuixue
		/// </summary>
		TimeUpHP = 2,
		/// <summary>
		/// yicixingjiamo
		/// </summary>
		OneUpMP = 3,
		/// <summary>
		/// chixujiamo
		/// </summary>
		TimeUpMP = 4,
		/// <summary>
		/// dingshen
		/// </summary>
		MakeCanNotMove = 5,
		/// <summary>
		/// jinzhichiyao
		/// </summary>
		MakeCanNotUseTool = 6,
		/// <summary>
		/// fengqinggong
		/// </summary>
		MakeCanNotJump = 7,
		/// <summary>
		/// hunluan
		/// </summary>
		MakeIsInChaos = 8,
		/// <summary>
		/// mihuo
		/// </summary>
		MakeDeluded = 9,
		/// <summary>
		/// mabi
		/// </summary>
		MakeParalyzed = 10,
		/// <summary>
		/// daodi
		/// </summary>
		MakeIsDown = 11
	}

	public enum WeaponType {
		/// <summary>
		/// kong shou
		/// </summary>
		EmptyHanded = 0,
		/// <summary>
		/// gun zi
		/// </summary>
		Stick = 1,
		/// <summary>
		/// jian
		/// </summary>
		Sword = 2,
		/// <summary>
		/// dao
		/// </summary>
		Knife = 3,
		/// <summary>
		/// qiang
		/// </summary>
		Spear = 4,
		/// <summary>
		/// fu tou
		/// </summary>
		Axe = 5,
		/// <summary>
		/// tui gong
		/// </summary>
		Leg = 6,
		/// <summary>
		/// an qi
		/// </summary>
		Darts = 7,
		/// <summary>
		/// fei dun
		/// </summary>
		FlyShield = 8,
		/// <summary>
		/// bian zi
		/// </summary>
		Whip = 9,
		/// <summary>
		/// gong nu
		/// </summary>
		Bow = 10
	}
}
