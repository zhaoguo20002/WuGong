using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using PigeonCoopToolkit.Navmesh2D;
using DG.Tweening;
using DG.Tweening.Core;
using Game;

public class RoleCtrl : MonoBehaviour {
	/// <summary>
	/// The is host.
	/// </summary>
	public bool IsHost;
	/// <summary>
	/// shifoudingshen
	/// </summary>
	public bool CanNotMove;
	/// <summary>
	/// shifoujinzhinengchiyao
	/// </summary>
	public bool CanNotUseTool;
	/// <summary>
	/// shifoufengsuoqinggong
	/// </summary>
	public bool CanNotJump;
	/// <summary>
	/// shifouhunluan
	/// </summary>
	public bool IsInChaos;
	/// <summary>
	/// shifoumihuo
	/// </summary>
	public bool Deluded;
	/// <summary>
	/// shifoumabi
	/// </summary>
	public bool Paralyzed;
	/// <summary>
	/// shifoudaodi
	/// </summary>
	public bool IsDown;
	/// <summary>
	/// The role data.
	/// </summary>
	public RoleData RoleData;
	
	List<Vector2> path;
	float speed = 2;

	tk2dSpriteAnimator ani;
	Animator weaponAin;
	Vector3 lastPosition;
	string curClipName;
	float speedRate;

	string states;
	float singDate;
	float singTimeout;
	string doActionNameFix;
	string doActionClipName;
	System.Action doActionCallback;
	SkillData currentSkill;
	bool canUseSkill;
	bool flying;

	Transform weapon;

//	NavMesh2DBehaviour navMesh2D;
//	List<NavMesh2DConnection> navMesh2DConnections;
	// Use this for initialization
	void Awake () {
		path = new List<Vector2>();
		ani = GetComponent<tk2dSpriteAnimator>();
		ani.SetFrame(0);
		weaponAin = GetComponentInChildren<Animator>();
//		ani.AnimationCompleted = completeCallback;
		transform.position = Statics.GetMapResetPosition(transform.position);
		lastPosition = transform.position;
//		navMesh2D = NavMesh2D.GetNavMeshObject();
		curClipName = "down";

		CanNotMove = false;
		CanNotUseTool = false;
		CanNotJump = false;
		IsInChaos = false;
		Deluded = false;
		Paralyzed = false;
		IsDown = false;

		states = "normal";
		singDate = Time.fixedTime;
		doActionNameFix = "";
		canUseSkill = true;
		flying = false;

		weapon = transform.FindChild("weapon");

		SetSpeed(1);

	}

//	public void GetConnectedNode() {
//		navMesh2DConnections = navMesh2D.ActualClosestNodeTo(transform.position).connections;
//		Debug.LogWarning(navMesh2DConnections.Count);
//		foreach (NavMesh2DConnection connection in navMesh2DConnections)
//		{
//			NavMesh2DNode connectedMesh2DNode = navMesh2D.GetNode(connection.connectedNodeIndex);
//			Debug.LogWarning(Vector3.Distance(transform.position, connectedMesh2DNode.position) + "," + connectedMesh2DNode.position);
//		}
//		//transform.position = navMesh2D.GetNode(navMesh2DConnections[0].connectedNodeIndex).position;
//	}

//	void completeCallback(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip) {
//		if (doActionCallback != null) {
//			doActionCallback();
//			doActionCallback = null;
//		}
//		ani.Play(curClipName);
//		states = "normal";
//		MakeStop();
//	}

	/// <summary>
	/// Makes the move.
	/// </summary>
	/// <param name="endPostion">End postion.</param>
	public void MakeMove(Vector3 endPostion) {
		path = NavMesh2D.GetSmoothedPath(transform.position, endPostion);
		if (path.Count == 0) {
			MakeStop();
		}
	}

	/// <summary>
	/// Sets the path.
	/// </summary>
	/// <param name="movePath">Move path.</param>
	public void SetPath(List<Vector2> movePath) {
		StopSkill();
		path = movePath;
		if (path.Count == 0) {
			MakeStop();
		}
	}

	/// <summary>
	/// Makes the stop.
	/// </summary>
	public void MakeStop() {
		path.Clear();
		ani.Stop();
		ani.SetFrame(0);
		states = "normal";
	}

	/// <summary>
	/// Sets the speed.
	/// </summary>
	/// <param name="speed">Speed.</param>
	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
		speedRate = speed * 6;
		if (ani.CurrentClip != null) {
			ani.CurrentClip.fps = speedRate;
		}
		ani.GetClipByName("up").fps = speedRate;
		ani.GetClipByName("right").fps = speedRate;
		ani.GetClipByName("down").fps = speedRate;
		ani.GetClipByName("left").fps = speedRate;
		
		ani.GetClipByName("round").fps = speedRate;

		ani.GetClipByName("upAttack").fps = speedRate;
		ani.GetClipByName("rightAttack").fps = speedRate;
		ani.GetClipByName("downAttack").fps = speedRate;
		ani.GetClipByName("leftAttack").fps = speedRate;
	}

	string getDirction(Vector3 fromPosition, Vector3 toPosition) {
		float angle = Mathf.Atan2(fromPosition.x - toPosition.x, fromPosition.y - toPosition.y) / Mathf.PI * 180;
		angle = angle >= 0 ? angle : angle + 360;
		if (angle <= 15 || angle > 345) {
			return "up";
		}
		else if (angle > 15 && angle <= 165) {
			return "right";
		}
		else if (angle > 165 && angle <= 195) {
			return "down";
		}
		else if (angle > 195 && angle <= 345)  {
			return "left";
		}
		return "";
	}

	/// <summary>
	/// Moves the towards.
	/// </summary>
	/// <returns><c>true</c>, if towards was moved, <c>false</c> otherwise.</returns>
	/// <param name="toPosition">To position.</param>
	public bool MoveTowards(Vector3 toPosition) {
		Vector3 resultPostion = Statics.GetMapResetPosition(toPosition);
		if (resultPostion != transform.position) {
			transform.position = Vector2.MoveTowards(transform.position, resultPostion, speed * Time.deltaTime);
			curClipName = getDirction(transform.position, lastPosition);
			ani.Play(curClipName);
			lastPosition = transform.position;
		}
		if (Vector2.Distance(transform.position, resultPostion) < 0.01f) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Jump the specified aimPosition, jumpPower, numJumps and during.
	/// </summary>
	/// <param name="aimPosition">Aim position.</param>
	/// <param name="jumpPower">Jump power.</param>
	/// <param name="numJumps">Number jumps.</param>
	/// <param name="during">During.</param>
	public void Jump(Vector3 aimPosition, float jumpPower = 1, int numJumps = 1, float during = 1) {
		if (CanNotJump) {
			return;
		}
		StopSkill();
		flying = true;
		states = "jump";
		ani.GetClipByName("round").fps = speedRate;
		ani.Play("round");
		transform.DOKill(false);
		transform.DOJump(aimPosition, jumpPower, numJumps, during)
		.OnComplete(() => {
			flying = false;
			ani.Play(curClipName);
			MakeStop();
			StopSkill();
		});
	}

	/// <summary>
	/// Dos the action.
	/// </summary>
	/// <param name="toPosition">To position.</param>
	/// <param name="callback">Callback.</param>
	/// <param name="singTime">Sing time.</param>
	/// <param name="actionNameFix">Action name fix.</param>
	public void DoAction(Vector3 toPosition, System.Action callback, float singTime = 0, string actionNameFix = "Attack") {
		singTime = Mathf.Clamp(singTime, 0, 10f);
		//only not sing time skill can shoot when jump
		if (flying && singTime > 0) {
			StopSkill();
			return;
		}
		states = "doAction";
		doActionCallback = callback;
		singTimeout = singTime;
		singDate = Time.fixedTime;
		curClipName = getDirction(toPosition, transform.position);
		doActionNameFix = actionNameFix;
		doActionClipName = curClipName + doActionNameFix;

		if (singTime > 0) {
			ani.GetClipByName("round").fps = 2f / singTime * 6f;
			ani.Play("round");
			switch (curClipName) {
			case "down":
			default:
				ani.PlayFromFrame(0);
				break;
			case "left":
				ani.PlayFromFrame(1);
				break;
			case "up":
				ani.PlayFromFrame(2);
				break;
			case "right":
				ani.PlayFromFrame(3);
				break;
			}
		}
	}

	/// <summary>
	/// Uses the skill.
	/// </summary>
	/// <param name="skill">Skill.</param>
	public void UseSkill(SkillData skill) {
		if (!canUseSkill) {
			return;
		}
		canUseSkill = false;
		currentSkill = skill;
		weapon.DOKill();
		DoAction(currentSkill.BulletDatas[0].ToPosition, null, currentSkill.SingTime, "Attack");
	}

	/// <summary>
	/// Stops the skill.
	/// </summary>
	public void StopSkill() {
		canUseSkill = true;
		currentSkill = null;
		singTimeout = 0;
		if (!flying) {
			states = "normal";
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		switch (states) {
		case "normal":
			if(!CanNotMove && path != null && path.Count != 0)
			{
				if(MoveTowards(path[0]))
				{
					path.RemoveAt(0);
					if (path.Count == 0) {
						MakeStop();
					}
				}
			}
			break;
		case "doAction":
			if (Time.fixedTime - singDate >= singTimeout) {
				singDate = Time.fixedTime;
				states = "doActing";
				ani.Play(doActionClipName);
				weaponAin.Play("StickAttack0_" + curClipName);
			}
			break;
		case "doActing":
			if (ani.CurrentFrame == 1) {
				if (doActionNameFix == "Attack" && currentSkill != null) {
//					if (RoleData.WeaponData.Type == WeaponType.EmptyHanded) {
						states = "doActionEnd";
						for (int i = 0; i < currentSkill.BulletDatas.Count; i++) {
							Messenger.Broadcast<BulletData>(NotifyTypes.ShootBullet, currentSkill.BulletDatas[i]);
						}
						currentSkill = null;
						doActionNameFix = "";
//					}
//					else {
//						states = "checkingWeapon";
////						weapon.DOLocalRotate(new Vector3(0, 0, -90), 0.5f)
//						weapon.DOPunchRotation(new Vector3(0, 0, -90), 0.2f)
//							.OnComplete(() => {
//								states = "doActionEnd";
//								Messenger.Broadcast<BulletData>(NotifyTypes.ShootBullet, currentSkill.Bullet);
//								currentSkill = null;
//								doActionNameFix = "";
//							});
//					}
				}
			}
			break;
		case "checkingWeapon":

			break;
		case "doActionEnd":
			Debug.LogWarning("doActionEnd, " + ani.CurrentFrame);
			if (ani.CurrentFrame >= ani.CurrentClip.frames.Length - 1) {
				endAction();
			}
			break;
		case "jump":

			break;
		default:
			break;
		}
	}

	void endAction() {
		if (doActionCallback != null) {
			doActionCallback();
			doActionCallback = null;
		}
		ani.Play(curClipName);
		states = "normal";
		MakeStop();
		canUseSkill = true;
	}

	//collision
//	void OnCollisionEnter2D(Collision2D other) {
//		Debug.LogWarning(transform.name + ", collision enter - " + other.gameObject.name);	
//	}
//
//	void OnCollisionStay2D(Collision2D other) {
//		Debug.LogWarning(transform.name + ", collision stay - " + other.gameObject.name);	
//	}
//
//	void OnCollisionExit2D(Collision2D other) {
//		Debug.LogWarning(transform.name + ", collision exit - " + other.gameObject.name);	
//	}

////	//trigger
//	void OnTriggerEnter2D(Collider2D other) {
//		//Debug.LogWarning(transform.name + ", trigger enter - " + other.gameObject.name);	
//	}
//
//	void OnTriggerStay2D(Collider2D other) {
//		//Debug.LogWarning(transform.name + ", trigger stay - " + other.gameObject.name);	
//	}
//	
//	void OnTriggerExit2D(Collider2D other) {
////		Debug.LogWarning(transform.name + ", trigger exit - " + other.gameObject.name);	
//	}
//
//	void OnTriggerEnter(Collider other) {
//		Debug.LogWarning(transform.name + ", OnTriggerEnter - " + other.gameObject.name);	
//	}

//	void OnDrawGizmos() {
//		if (navMesh2DConnections != null) {
//			Gizmos.color = Color.red;
//			foreach (NavMesh2DConnection connection in navMesh2DConnections)
//			{
//				NavMesh2DNode connectedMesh2DNode = navMesh2D.GetNode(connection.connectedNodeIndex);
//				Gizmos.DrawLine(transform.position, connectedMesh2DNode.position);
//			}
//			
//			Vector3 p11 = new Vector3(1, 1, 0);
//			Vector3 p12 = new Vector3(1.1f, 1.9f, 0);
//			Vector3 p21 = new Vector3(1, 2, 0);
//			Vector3 p22 = new Vector3(2, 1, 0);
//			Gizmos.color = Color.green;
//			Gizmos.DrawLine(p11, p12);
//			Gizmos.DrawLine(p21, p22);
//			if (Statics.IsLineMeeting(p12, p11, p21, p22)) {
//				Vector3 tntersection = Statics.GetIntersection(p12, p11, p21, p22);
//				Gizmos.color = Color.yellow;
//				Gizmos.DrawLine(transform.position, tntersection);
//			}
//
//		}
//	}
}
