using UnityEngine;
using System.Collections;

namespace Game {
	public class RoleData {
		string _id;
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return _id;
			}
		}
		string _name;
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return _name;
			}
		}
		/// <summary>
		/// The start position.
		/// </summary>
		public Vector3 StartPosition;
		/// <summary>
		/// The sprite source.
		/// </summary>
		public string SpriteSrc;
		/// <summary>
		/// The weapon data.
		/// </summary>
		public WeaponData WeaponData;
		public bool IsHost;
		/// <summary>
		/// The speed.
		/// </summary>
		public float Speed;
		/// <summary>
		/// The attack.
		/// </summary>
		public int Attack;
		/// <summary>
		/// The defense.
		/// </summary>
		public int Defense;

		public RoleData (string id, string name, Vector3 startPosition, string spriteSrc, WeaponData weaponData, bool isHost = false, float speed = 1.5f, int attack = 100, int defense = 100) {
			_id = id;
			_name = name;
			IsHost = isHost;
			StartPosition = startPosition;
			SpriteSrc = spriteSrc;
			WeaponData = weaponData;
			Speed = speed;
			Attack = attack;
			Defense = defense;
		}
	}
}