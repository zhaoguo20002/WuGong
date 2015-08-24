using UnityEngine;
using System.Collections;
using Game;

public class Main : MonoBehaviour {

	void Awake() {
		Messenger.Broadcast(NotifyTypes.InitRolesCtrl);
	}

	// Use this for initialization
	void Start () {
		Messenger.Broadcast<RoleData[]>(NotifyTypes.AppendRoles, new RoleData[]{
			new RoleData("role0", "role0", new Vector3(5, 5, 0), "Prefabs/Roles/AnimatedSprite", new WeaponData(WeaponType.Stick, ""), true),
			new RoleData("role1", "role1", new Vector3(6, 6, 0), "Prefabs/Roles/AnimatedSprite", new WeaponData(WeaponType.Stick, ""))
		});

		Messenger.Broadcast<string>(NotifyTypes.SetHost, "role0");
	}
}
