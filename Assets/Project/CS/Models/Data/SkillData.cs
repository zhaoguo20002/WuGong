using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Game {
	public class SkillData {
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

		string _skillName;
		/// <summary>
		/// Gets the name of the skill.
		/// </summary>
		/// <value>The name of the skill.</value>
		public string SkillName {
			get {
				return _skillName;
			}
		}
		
		
		/// <summary>
		/// The bullet datas.
		/// 可能有多发子弹
		/// </summary>
		public List<BulletData> BulletDatas;
		
		/// <summary>
		/// The is track.
		/// 是否追踪
		/// </summary>
		public bool IsTrack;

		/// <summary>
		/// The sing time.
		/// </summary>
		public float SingTime;

		public SkillData (string id, string skillName, List<BulletData> bulletDatas, float singTime) {
			_id = id;
			_skillName = skillName;
			BulletDatas = bulletDatas;
			SingTime = singTime;
		}
	}
}

