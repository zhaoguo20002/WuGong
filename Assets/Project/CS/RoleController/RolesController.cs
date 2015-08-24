using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;
using Newtonsoft.Json.Linq;
using DG.Tweening;
using DG.Tweening.Core;

public class RolesController : MonoBehaviour {
	public Transform Host;
	public Camera myCamera;
	public float followSpeed = 10;
	RoleCtrl roleCtrl;
	float distance = 20;
	int layerMask = 1 << 10;
	Ray ray;
	RaycastHit hit;
	Vector3 endPostion;
	static Dictionary<string, bool> obstacleMapping;
	List<Vector2> runPath;

	//角色相关
	Dictionary<string, RoleCtrl> roleCtrls;
	Dictionary<string, UnityEngine.Object> roleSpritePrefabs;

	//特效相关
	Dictionary<string, UnityEngine.Object> effectsResMapping;
	Dictionary<string, UnityEngine.Object> haloResMapping;
	Dictionary<string, GameObject> effectsObjMapping;
	Dictionary<string, GameObject> halosObjMapping;
	Dictionary<string, BuffData> buffDataObjMapping;

	//战斗相关
	Dictionary<string, UnityEngine.Object> bulletsResMapping;
	Dictionary<string, UnityEngine.Object> flaresResMapping;
	List<GameObject> destoryList;
	List<Bullet> bullets;

	// Use this for initialization
	void Start () {
		Init();
		roleCtrls = new Dictionary<string, RoleCtrl>();
		roleSpritePrefabs = new Dictionary<string, Object>();
		effectsResMapping = new Dictionary<string, Object>();
		haloResMapping = new Dictionary<string, Object>();
		effectsObjMapping = new Dictionary<string, GameObject>();
		halosObjMapping = new Dictionary<string, GameObject>();
		buffDataObjMapping = new Dictionary<string, BuffData>();
		bulletsResMapping = new Dictionary<string, Object>();
		flaresResMapping = new Dictionary<string, Object>();
		destoryList = new List<GameObject>();
		bullets = new List<Bullet>();
	}
	
	// Update is called once per frame
	void Update() {
		Bullet bullet;
		for (int i = bullets.Count - 1; i >= 0; i--) {
			bullet = bullets[i];
			if (bullet != null && bullet.IsDied) {
				bullet.Bomb();
				destoryList.Add(bullet.gameObject);
				bullets.RemoveAt(i);
			}
		}
	}
	
	void LateUpdate () {
		if (roleCtrl != null) {
			Vector3 start = myCamera.transform.position;
			Vector3 end = Vector3.MoveTowards(start, Host.position, followSpeed * Time.deltaTime);
			end.z = start.z;
			myCamera.transform.position = end;
			if (endPostion !=  Vector3.zero) {
				roleCtrl.MakeMove(new Vector3(endPostion.x, endPostion.y, 0));
				endPostion = Vector3.zero;
			}
		}
		for (int i = destoryList.Count - 1; i >= 0; i--) {
			if (destoryList[i] != null) {
				MonoBehaviour.Destroy(destoryList[i]);
			}
		}
	}
	
	void OnGUI () {
		Vector3 position;
		if (Input.touchCount > 0) {
			position = Input.GetTouch(0).position;
		}
		else {
			position = Input.mousePosition;
		}
		if (Event.current.type == EventType.MouseDown) {
			//创建鼠标射线
			ray = myCamera.ScreenPointToRay(position);
			//判定射线碰撞点
			if (Physics.Raycast(ray, out hit, distance, layerMask)) {
				//endPostion = hit.point;
			}
		}
		else if (Event.current.type == EventType.MouseDrag) {
			//Debug.LogWarning("MouseDrag");
			//创建鼠标射线
			ray = myCamera.ScreenPointToRay(position);
			//判定射线碰撞点
			if (Physics.Raycast(ray, out hit, distance, layerMask)) {
				runPath = getRunPath(Host.transform.position, hit.point);
				roleCtrl.SetPath(runPath);
			}
		}
		//		else if (Event.current.type == EventType.MouseUp) {
		//roleCtrl.MakeStop();
		//		}
		
		if (GUI.Button(new Rect(50, 50, 100, 50), "+ Speed")) {
			roleCtrl.SetSpeed(2);
		}
		if (GUI.Button(new Rect(160, 50, 100, 50), "go 1")) {
			Messenger.Broadcast<string>(NotifyTypes.GoToScene, "Scene001");
		}
		if (GUI.Button(new Rect(270, 50, 100, 50), "go 2")) {
			Messenger.Broadcast<string>(NotifyTypes.GoToScene, "Scene002");
		}
		if (GUI.Button(new Rect(50, 120, 100, 50), "shoot")) {
			Messenger.Broadcast<JArray>(NotifyTypes.ShootBulletEcho, new JArray(roleCtrl.name, "role1", 100, 1, 10, 10, 0, 0, "SigleBullet", "flare 22"));
		}
		if (GUI.Button(new Rect(160, 120, 100, 50), "Jump")) {
			roleCtrl.Jump(new Vector3(8, 8, 0), 0.5f, 5, 3);
		}
	}

	List<Vector2> getRunPath(Vector3 startPosition, Vector3 toPosition) {
		List<Vector2> _path = new List<Vector2>();
		int curRow = Statics.GetMapRowIndex(startPosition.y);
		int curCol = Statics.GetMapColIndex(startPosition.x);
		int endRow = Statics.GetMapRowIndex(toPosition.y);
		int endCol = Statics.GetMapRowIndex(toPosition.x);
		int upgradeRow = endRow - curRow;
		upgradeRow = upgradeRow != 0 ? (upgradeRow / Mathf.Abs(upgradeRow)) : 0;
		int upgradeCol = endCol - curCol;
		upgradeCol = upgradeCol != 0 ? (upgradeCol / Mathf.Abs(upgradeCol)) : 0;
		bool isEnd = false;
		while(!isEnd) {
			isEnd = true;
			if (curRow != endRow || curCol != endCol) {
				if (upgradeRow != 0 && upgradeCol!= 0 && !obstacleMapping.ContainsKey((curRow + upgradeRow) + "_" + (curCol + upgradeCol)) && (!obstacleMapping.ContainsKey((curRow + upgradeRow) + "_" + curCol) && !obstacleMapping.ContainsKey(curRow + "_" + (curCol + upgradeCol)))) {
					if (curRow != endRow) {
						curRow += upgradeRow;
						isEnd = false;
					}
					if (curCol != endCol) {
						curCol += upgradeCol;
						isEnd = false;
					}
				}
				else if (upgradeRow != 0 && !obstacleMapping.ContainsKey((curRow + upgradeRow) + "_" + curCol)) {
					if (curRow != endRow) {
						curRow += upgradeRow;
						isEnd = false;
					}
				}
				else if (upgradeCol != 0 && !obstacleMapping.ContainsKey(curRow + "_" + (curCol + upgradeCol))) {
					if (curCol != endCol) {
						curCol += upgradeCol;
						isEnd = false;
					}
				}
				
				if (!isEnd) {
					_path.Add(new Vector2(Statics.GetMapX(curCol), Statics.GetMapY(curRow)));
				}

			}
			if (isEnd) {
				if (!obstacleMapping.ContainsKey(curRow + "_" + curCol)) {
					_path.Add(new Vector2(Statics.GetMapX(curCol), Statics.GetMapY(curRow)));
					
				}
			}
		}
		return _path;
	}

	public void Init() {
		obstacleMapping = Statics.GetObstacle(Application.loadedLevelName);
		if (obstacleMapping == null) {
			return;
		}
		if (myCamera == null) {
			myCamera = Camera.main;
		}
		if (Host != null) {
			roleCtrl = Host.GetComponent<RoleCtrl>();
		}
		endPostion = Vector3.zero;
	}

	/// <summary>
	/// Repel the specified toRoleId, angle and num.
	/// </summary>
	/// <param name="toRoleId">To role identifier.</param>
	/// <param name="angle">Angle.</param>
	/// <param name="num">Number.</param>
	public void Repel(string toRoleId, float angle, float num) {
		GameObject toRole = GameObject.Find(toRoleId);
		if (toRole != null) {
			Vector3 toPosition = Statics.GetCirclePoint(toRole.transform.position, 0.4525483f * num, angle);
			List<Vector2> resultPath = getRunPath(toRole.transform.position, toPosition);
			if (resultPath.Count > 0) {
				Vector2 lastPoint = resultPath[resultPath.Count - 1];
				toRole.transform.DOMove(new Vector3(lastPoint.x, lastPoint.y, 0), 0.5f)
					.SetUpdate(true)
						.SetEase(Ease.OutBack);

			}
		}
	}

	/// <summary>
	/// Adds the buff.
	/// </summary>
	/// <param name="buffData">Buff data.</param>
	public void AddBuff(BuffData buffData) {
		RemoveBuff(buffData.Id);
		GameObject toRole = GameObject.Find(buffData.ToId);
		if (toRole != null) {
			if (buffData.EffectResName != "") {
				if (!effectsResMapping.ContainsKey(buffData.EffectResName)) {
					effectsResMapping.Add(buffData.EffectResName, Statics.GetPrefab("Prefabs/Effects/" + buffData.EffectResName));
				}
				if (!effectsObjMapping.ContainsKey(buffData.Id)) {
					effectsObjMapping[buffData.Id] = MonoBehaviour.Instantiate(effectsResMapping[buffData.EffectResName]) as GameObject;
					if (effectsObjMapping[buffData.Id] != null) {
						effectsObjMapping[buffData.Id].transform.parent = toRole.transform.FindChild("Effects");
						effectsObjMapping[buffData.Id].transform.localPosition = Vector3.zero;
						effectsObjMapping[buffData.Id].layer = Statics.GetEffectLayer();
					}
				}
			}
			if (buffData.HaloResName != "") {
				if (!haloResMapping.ContainsKey(buffData.HaloResName)) {
					haloResMapping.Add(buffData.HaloResName, Statics.GetPrefab("Prefabs/Halos/" + buffData.HaloResName));
				}
				if (!halosObjMapping.ContainsKey(buffData.Id)) {
					halosObjMapping[buffData.Id] = MonoBehaviour.Instantiate(haloResMapping[buffData.HaloResName]) as GameObject;
					if (halosObjMapping[buffData.Id] != null) {
						halosObjMapping[buffData.Id].transform.parent = toRole.transform.FindChild("Halos");
						halosObjMapping[buffData.Id].transform.localPosition = Vector3.zero;
						halosObjMapping[buffData.Id].layer = Statics.GetHaloLayer();
					}
				}
			}
			Timer.AddTimer(buffData.Id, buffData.Second, null, (timer) => {
				RemoveBuff(timer.Id);
			});
			buffDataObjMapping.Add(buffData.Id, buffData); //save buff data
		}
	}

	/// <summary>
	/// Removes the buff.
	/// </summary>
	/// <param name="buffId">Buff identifier.</param>
	public void RemoveBuff(string buffId) {
		if (buffDataObjMapping.ContainsKey(buffId)) {
			BuffData buffData = buffDataObjMapping[buffId];
			//remove effect obj
			if (effectsObjMapping.ContainsKey(buffId)) {
				Destroy(effectsObjMapping[buffId]);
				effectsObjMapping.Remove(buffId);
			}
			//remove halo obj
			if (halosObjMapping.ContainsKey(buffId)) {
				Destroy(halosObjMapping[buffId]);
				halosObjMapping.Remove(buffId);
			}
			buffDataObjMapping.Remove(buffId);
			
			Timer.RemoveTimer(buffId);
		}
	}

	/// <summary>
	/// Uses the skill.
	/// </summary>
	/// <param name="skill">Skill.</param>
	public void UseSkill(SkillData skill) {
		if (roleCtrls[skill.Id] != null) {
			roleCtrls[skill.Id].UseSkill(skill);
		}
	}

	/// <summary>
	/// Appends the role.
	/// </summary>
	/// <param name="roleData">Role data.</param>
	public void AppendRole(RoleData roleData) {
		RemoveRole(roleData.Id);
		if (!roleSpritePrefabs.ContainsKey(roleData.SpriteSrc)) {
			roleSpritePrefabs[roleData.SpriteSrc] = Statics.GetPrefab(roleData.SpriteSrc);
		}
		GameObject role = Statics.GetPrefabClone(roleSpritePrefabs[roleData.SpriteSrc]);
		if (role != null) {
			RoleCtrl ctrl = role.GetComponent<RoleCtrl>();
			if (ctrl != null) {
				role.transform.position = roleData.StartPosition;
				role.transform.eulerAngles = Vector3.zero;
				role.name = roleData.Id;
				ctrl.IsHost = roleData.IsHost;
				ctrl.RoleData = roleData;
				roleCtrls.Add(roleData.Id, ctrl);
			}
		}
	}

	/// <summary>
	/// Removes the role.
	/// </summary>
	/// <param name="roleId">Role identifier.</param>
	public void RemoveRole(string roleId) {
		if (roleCtrls.ContainsKey(roleId)) {
			if (roleCtrls[roleId] != null && roleCtrls[roleId].gameObject != null) {
				Destroy(roleCtrls[roleId].gameObject);
			}
			roleCtrls.Remove(roleId);
		}
	}
	
	/// <summary>
	/// Clears the roles.
	/// </summary>
	public void ClearRoles() {
		foreach(RoleCtrl ctrl in roleCtrls.Values) {
			if (ctrl != null && ctrl.gameObject != null) {
				Destroy(ctrl.gameObject);
			}
		}
		roleCtrls.Clear();
	}

	/// <summary>
	/// Sets the host.
	/// </summary>
	/// <param name="roleId">Role identifier.</param>
	public void SetHost(string roleId) {
		if (roleCtrls.ContainsKey(roleId)) {
			roleCtrl = roleCtrls[roleId];
			Host = roleCtrl.transform;
		}
	}

	/// <summary>
	/// Gets the role.
	/// </summary>
	/// <returns>The role.</returns>
	/// <param name="roleId">Role identifier.</param>
	public RoleCtrl GetRole(string roleId) {
		return roleCtrls[roleId];
	}

	/// <summary>
	/// Gets the host role.
	/// </summary>
	/// <returns>The host role.</returns>
	public RoleCtrl GetHostRole() {
		return roleCtrl;
	}

	/// <summary>
	/// Shoots the bullet.
	/// </summary>
	/// <param name="data">Data.</param>
	public void ShootBullet(BulletData data) {
		if (!bulletsResMapping.ContainsKey(data.BulletResName)) {
			bulletsResMapping.Add(data.BulletResName, Statics.GetPrefab("Prefabs/Bullets/" + data.BulletResName));
		}
		GameObject bulletObj = MonoBehaviour.Instantiate(bulletsResMapping[data.BulletResName]) as GameObject;
		Bullet bullet = bulletObj.GetComponent<Bullet>();
		if (bullet != null) {
			bullet.bulletData = data;
			bullet.Shoot(data.FromPosition, data.ToPosition);
			bullets.Add(bullet);
		}
	}
	
	/// <summary>
	/// Creates the flare.
	/// </summary>
	/// <param name="toPosition">To position.</param>
	/// <param name="injureValue">Injure value.</param>
	/// <param name="flaresResName">Flares res name.</param>
	public void CreateFlare(Vector3 toPosition, int injureValue, string flaresResName) {
		if (!flaresResMapping.ContainsKey(flaresResName)) {
			flaresResMapping.Add(flaresResName, Statics.GetPrefab("Prefabs/Flares/" + flaresResName));
		}
		GameObject flareObj = MonoBehaviour.Instantiate(flaresResMapping[flaresResName]) as GameObject;
		if (flareObj != null) {
			flareObj.transform.position = toPosition;
			flareObj.layer = Statics.GetFlareLayer();
		}
	}

	/// <summary>
	/// Disposed this instance.
	/// </summary>
	public void Disposed() {
		obstacleMapping.Clear();
		obstacleMapping = null;
		if (runPath != null) {
			runPath.Clear();
		}
		runPath = null;
		ClearRoles();
		roleSpritePrefabs.Clear();
		foreach(string buffId in buffDataObjMapping.Keys) {
			Timer.RemoveTimer(buffId); //remove timer
			//remove effect obj
			if (effectsObjMapping.ContainsKey(buffId)) {
				Destroy(effectsObjMapping[buffId]);
				effectsObjMapping.Remove(buffId);
			}
			//remove halo obj
			if (halosObjMapping.ContainsKey(buffId)) {
				Destroy(halosObjMapping[buffId]);
				halosObjMapping.Remove(buffId);
			}
		}
		buffDataObjMapping.Clear();
		effectsResMapping.Clear();
		haloResMapping.Clear();

		bulletsResMapping.Clear();
		flaresResMapping.Clear();
		destoryList.Clear();
		bullets.Clear();
	}

//	void OnFingerMove(FingerMotionEvent e) { 
//		if( e.Phase == FingerMotionPhase.Started )
//		{
//			Debug.LogWarning("Started start " + Camera.main.ScreenToWorldPoint(e.Position));
//		}
//		else if( e.Phase == FingerMotionPhase.Updated )
//		{
//			Debug.LogWarning("Started updated " + Camera.main.ScreenToWorldPoint(e.Position));
//			//roleCtrl.SetMovingToPosition(Camera.main.ScreenToWorldPoint(e.Position));
//		}
//		else
//		{
//			Debug.LogWarning("Stopped end " + Camera.main.ScreenToWorldPoint(e.Position));
//			//endPostion = Camera.main.ScreenToWorldPoint(e.Position);
//			//roleCtrl.SetMovingToPosition(Camera.main.ScreenToWorldPoint(e.Position));
//		}
//	}

	void OnDrawGizmos() {
		if (runPath == null || runPath.Count == 0) {
			return;
		}
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Host.transform.position, runPath[0]);
		for (int i = 0; i < runPath.Count - 1; i++) {
			Gizmos.DrawLine(runPath[i], runPath[i + 1]);
		}
	}
}
