using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Game {
	public partial class NotifyTypes {
		/// <summary>
		/// The init roles ctrl.
		/// </summary>
		public static string InitRolesCtrl;
		/// <summary>
		/// The append role.
		/// </summary>
		public static string AppendRole;
		/// <summary>
		/// The append roles.
		/// </summary>
		public static string AppendRoles;
		/// <summary>
		/// The rmove role.
		/// </summary>
		public static string RemoveRole;
		/// <summary>
		/// The remove roles.
		/// </summary>
		public static string RemoveRoles;
		/// <summary>
		/// The clear roles.
		/// </summary>
		public static string ClearRoles;
		/// <summary>
		/// The roles disposed.
		/// </summary>
		public static string RolesDisposed;
		/// <summary>
		/// The set host.
		/// </summary>
		public static string SetHost;
		/// <summary>
		/// The shoot bullet echo.
		/// </summary>
		public static string ShootBulletEcho;
		/// <summary>
		/// The shoot bullet.
		/// </summary>
		public static string ShootBullet;
		/// <summary>
		/// The fight disposed.
		/// </summary>
		public static string FightDisposed;
		/// <summary>
		/// The bullet hited.
		/// </summary>
		public static string BulletHited;
	}
	public partial class NotifyRegister {
		/// <summary>
		/// Scenes the notify init.
		/// </summary>
		public static void RoleNotifyInit() {
			Messenger.AddListener(NotifyTypes.InitRolesCtrl, () => {
				if (RoleModel.RolesCtrl == null) {
					RoleModel.RolesCtrl = GameObject.Find("HostController").GetComponent<RolesController>();
				}
				RoleModel.RolesCtrl.Init();
			});

			Messenger.AddListener<RoleData>(NotifyTypes.AppendRole, (roleData) => {
				if (RoleModel.RolesCtrl != null) {
					RoleModel.RolesCtrl.AppendRole(roleData);
				}
			});

			Messenger.AddListener<RoleData[]>(NotifyTypes.AppendRoles, (roleDatas) => {
				if (RoleModel.RolesCtrl != null) {
					foreach(RoleData data in roleDatas) {
						RoleModel.RolesCtrl.AppendRole(data);
					}
				}
			});

			Messenger.AddListener<string>(NotifyTypes.RemoveRole, (roleId) => {
				if (RoleModel.RolesCtrl != null) {
					RoleModel.RolesCtrl.RemoveRole(roleId);
				}
			});

			Messenger.AddListener<string[]>(NotifyTypes.RemoveRoles, (roleIds) => {
				if (RoleModel.RolesCtrl != null) {
					foreach(string roleId in roleIds) {
						RoleModel.RolesCtrl.RemoveRole(roleId);
					}
				}
			});

			Messenger.AddListener(NotifyTypes.ClearRoles, () => {
				if (RoleModel.RolesCtrl != null) {
					RoleModel.RolesCtrl.ClearRoles();
				}
			});

			Messenger.AddListener(NotifyTypes.RolesDisposed, () => {
				if (RoleModel.RolesCtrl != null) {
					RoleModel.RolesCtrl.Disposed();
				}
			});

			Messenger.AddListener<string>(NotifyTypes.SetHost, (roleId) => {
				if (RoleModel.RolesCtrl != null) {
					RoleModel.RolesCtrl.SetHost(roleId);
				}
			});
			
			Messenger.AddListener<JArray>(NotifyTypes.ShootBulletEcho, (data) => {
				string fromRoleId = data[0].ToString();
				string toRoleId = data[1].ToString();
				int injureValue = (int)data[2];
				int life = (int)data[3];
				float range = (float)data[4];
				float speed = (float)data[5];
				int sonBulletNum = (int)data[6];
				float injureIncreasingRat = (float)data[7];
				string bulletResName = data[8].ToString();
				string flaresResName = data[9].ToString();
				
				GameObject role = GameObject.Find(fromRoleId);
				if (role == null) {
					return;
				}
				Vector3 fromPosition = role.transform.position;
				role = GameObject.Find(toRoleId);
				if (role == null) {
					return;
				}
				Vector3 toPosition = role.transform.position;
				float distance = Vector3.Distance(fromPosition, toPosition);
				float angle = -(Statics.GetAngle(fromPosition.x, fromPosition.y, toPosition.x, toPosition.y) - 90);
				//out range
				//if (distance > range) {
				distance = range;
				Vector3 aim = Statics.GetCirclePoint(fromPosition, distance, angle);
				toPosition = new Vector3(aim.x, aim.y, 0);
				//}
				//				Messenger.Broadcast<BulletData>(NotifyTypes.ShootBullet, new BulletData(fromRoleId, 
				//				                                                                        fromPosition, 
				//				                                                                        toRoleId, 
				//				                                                                        toPosition, 
				//				                                                                        angle, 
				//				                                                                        BulletType.Normal, 
				//				                                                                        injureValue, 
				//				                                                                        new BuffData("", "", BuffType.MakeCanNotMove, 3),
				//				                                                                        life, 
				//				                                                                        distance, 
				//				                                                                        speed, 
				//				                                                                        sonBulletNum, 
				//				                                                                        injureIncreasingRat, 
				//				                                                                        bulletResName, 
				//				                                                                        flaresResName));
				RoleModel.RolesCtrl.UseSkill(new SkillData(fromRoleId, "xxx", new List<BulletData>() {
					new BulletData(fromRoleId, 
					               fromPosition, 
					               toRoleId, 
					               toPosition, 
					               angle, 
					               BulletType.Normal, 
					               true,
					               0,
					               0,
					               new BuffData("", "", BuffType.MakeCanNotMove, 3),
					               life, 
					               distance, 
					               speed, 
					               sonBulletNum, 
					               injureIncreasingRat, 
					               bulletResName, 
					               flaresResName)
				}, 0f));
			});
			
			Messenger.AddListener<BulletData>(NotifyTypes.ShootBullet, (data) => {
				if (RoleModel.RolesCtrl) {
					RoleModel.RolesCtrl.ShootBullet(data);
				}
			});
			
			Messenger.AddListener(NotifyTypes.FightDisposed, () => {
				if (RoleModel.RolesCtrl != null) {
					RoleModel.RolesCtrl.Disposed();
				}
			});
			
			Messenger.AddListener<BulletData>(NotifyTypes.BulletHited, (data) => {
				//这里用于处理子弹碰撞到对象后的处理逻辑
				//如是否miss，子弹是否到达目的地后爆炸等
				if (data.AffectedRoleIds.Count == 0) {
					//爆炸后的逻辑
					RoleModel.RolesCtrl.CreateFlare(data.ToPosition, 0, data.BombResName);
				}
				else {
					//是否miss,是否中debuff或者buff
					BuffData buffData = data.BuffData;
					foreach (string roleId in data.AffectedRoleIds.Keys) {
						RoleModel.RolesCtrl.CreateFlare(data.AffectedRoleIds[roleId], 0, data.FlaresResName);
						RoleModel.RolesCtrl.Repel(roleId, data.Angle, 3);
						RoleModel.RolesCtrl.AddBuff(new BuffData(data.Id + "_" + roleId + "_" + buffData.Type.ToString(), 
						                                         roleId, 
						                                         buffData.Type, 
						                                         buffData.Second, 
						                                         buffData.GetHP, 
						                                         buffData.GetMP, 
						                                         buffData.EffectResName, 
						                                         buffData.HaloResName));
					}
					data.AffectedRoleIds.Clear();
				}
			});
		}
	}
}
