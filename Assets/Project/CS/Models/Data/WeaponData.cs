using UnityEngine;
using System.Collections;

namespace Game {
	public class WeaponData {
		/// <summary>
		/// The type.
		/// </summary>
		public WeaponType Type;
		/// <summary>
		/// The sprite source.
		/// </summary>
		public string SpriteSrc;
		public WeaponData(WeaponType type, string spriteSrc) {
			Type = type;
			SpriteSrc = spriteSrc;
		}
	}
}
