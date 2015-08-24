using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game {
	public class BulletData {
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id;
		/// <summary>
		/// Gets from position.
		/// </summary>
		/// <value>From position.</value>
		public Vector3 FromPosition;
		/// <summary>
		/// Gets to identifier.
		/// 该属性只用来处理子弹追踪效果，其他时候子弹的目的坐标都以 ToPosition为准
		/// </summary>
		/// <value>To identifier.</value>
		public string ToId;
		/// <summary>
		/// To position.
		/// </summary>
		public Vector3 ToPosition;
		/// <summary>
		/// The angle.
		/// </summary>
		public float Angle;
		/// <summary>
		/// The type.
		/// </summary>
		public BulletType Type;
		/// <summary>
		/// The is real time.
		/// 是否在客户端实施演算伤害
		/// </summary>
		public bool IsRealTime;
		/// <summary>
		/// The got HP value.
		/// </summary>
		public int GotHPValue;
		/// <summary>
		/// The got MP value.
		/// </summary>
		public int GotMPValue;
		/// <summary>
		/// The buff.
		/// </summary>
		public BuffData BuffData;
		/// <summary>
		/// The life.
		/// </summary>
		public int Life;
		/// <summary>
		/// The range.
		/// </summary>
		public float Range;
		/// <summary>
		/// The speed.
		/// </summary>
		public float Speed;
		/// <summary>
		/// The son bullet number.
		/// </summary>
		public int SonBulletNum;
		/// <summary>
		/// The injure increasing rat.
		/// </summary>
		public float InjureIncreasingRat;
		/// <summary>
		/// The name of the bullet res.
		/// </summary>
		public string BulletResName;
		/// <summary>
		/// The name of the flares res.
		/// </summary>
		public string FlaresResName;
		/// <summary>
		/// The name of the bomb res.
		/// </summary>
		public string BombResName;
		/// <summary>
		/// The during.
		/// </summary>
		public float During {
			get {
				return Range / Speed;
			}
		}

		Dictionary<string, Vector3> affectedRoleIds;
		/// <summary>
		/// Gets the affected role identifiers.
		/// </summary>
		/// <value>The affected role identifiers.</value>
		public Dictionary<string, Vector3> AffectedRoleIds {
			get {
				return affectedRoleIds;
			}
		}

		public BulletData(string id, 
		                  Vector3 fromPosition,
		                  string toId,
		                  Vector3 toPosition,
		                  float angle,
		                  BulletType type, 
		                  bool isRealTime,
		                  int gotHPValue, 
		                  int gotMPValue,
		                  BuffData buffData,
		                  int life = 1, 
		                  float range = 10f,
		                  float speed = 10f,
		                  int sonBulletNum = 0, 
		                  float injureIncreasingRat = 0, 
		                  string bulletResName = "SigleBullet", 
		                  string flaresResName = "flare 22",
		                  string bombResName = "explosion 15") {
			Id = id;
			FromPosition = fromPosition;
			ToId = toId;
			ToPosition = toPosition;
			Angle = angle;
			Type = type;
			IsRealTime = isRealTime;
			GotHPValue = gotHPValue;
			GotMPValue = gotMPValue;
			BuffData = buffData;
			Life = life;
			Range = range;
			Speed = speed;
			SonBulletNum = sonBulletNum;
			InjureIncreasingRat = injureIncreasingRat;
			BulletResName = bulletResName;
			FlaresResName = flaresResName;
			BombResName = bombResName;
			affectedRoleIds = new Dictionary<string, Vector3>();
		}

		/// <summary>
		/// Appends the affected role identifier.
		/// </summary>
		/// <param name="roleId">Role identifier.</param>
		/// <param name="rolePosition">Role position.</param>
		public void AppendAffectedRoleId(string roleId, Vector3 rolePosition) {
			if (!affectedRoleIds.ContainsKey(roleId)) {
				affectedRoleIds.Add(roleId, rolePosition);
			}
		}

		/// <summary>
		/// Clears the affected role identifiers.
		/// </summary>
		public void ClearAffectedRoleIds() {
			affectedRoleIds.Clear();
		}
	}
}
