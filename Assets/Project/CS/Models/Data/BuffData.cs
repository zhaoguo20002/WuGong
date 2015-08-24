using UnityEngine;
using System.Collections;

namespace Game {
	public class BuffData {
		string _id = "";
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return _id;
			}
		}
		string _toId = "";
		/// <summary>
		/// Gets to identifier.
		/// </summary>
		/// <value>To identifier.</value>
		public string ToId {
			get {
				return _toId;
			}
		}
		/// <summary>
		/// The type.
		/// </summary>
		public BuffType Type;
		/// <summary>
		/// The second.
		/// </summary>
		public int Second;
		/// <summary>
		/// The get HP.
		/// </summary>
		public int GetHP;
		/// <summary>
		/// The get MP.
		/// </summary>
		public int GetMP;
		/// <summary>
		/// The name of the effect res.
		/// </summary>
		public string EffectResName;
		/// <summary>
		/// The name of the halo res.
		/// </summary>
		public string HaloResName;

		public BuffData(string id, string toId, BuffType type, int second, int getHP = 0, int getMP = 0, string effectResName = "sword arua 13", string haloResName = "ring 12") {
			_id = id;
			_toId = toId;
			Type = type;
			Second = second;
			GetHP = getHP;
			GetMP = getMP;
			EffectResName = effectResName;
			HaloResName = haloResName;
		}
	}
}
