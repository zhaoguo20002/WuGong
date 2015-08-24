using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Game;
using Newtonsoft.Json.Linq;

public class Bullet : MonoBehaviour {
	public BulletData bulletData;
	bool isDied;
	public bool IsDied {
		get {
			return isDied;
		}
	}

	Vector3 startPosition;
	Dictionary<string, Vector3> hitedOtherPosition;

	void Awake() {
		isDied = false;
		enabled = bulletData != null;
		hitedOtherPosition = new Dictionary<string, Vector3>();
	}

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}

	/// <summary>
	/// Shoot the specified fromPosition and toPosition.
	/// </summary>
	/// <param name="fromPosition">From position.</param>
	/// <param name="toPosition">To position.</param>
	public void Shoot(Vector3 fromPosition, Vector3 toPosition) {
		transform.position = new Vector3(fromPosition.x, fromPosition.y + 0.2f, startPosition.z);
		transform.DOMove(new Vector3(toPosition.x, toPosition.y + 0.2f, startPosition.z), bulletData.During)
			.SetUpdate(true)
			.SetEase(Ease.Linear)
			.OnComplete(() => {
				isDied = true;
			});
		transform.eulerAngles = new Vector3(0, 0, -Statics.GetAngle(transform.position.x, transform.position.y, toPosition.x, toPosition.y));
	}

	/// <summary>
	/// Bomb this instance.
	/// </summary>
	public void Bomb() {
		Messenger.Broadcast<BulletData>(NotifyTypes.BulletHited, bulletData);
	}

	void OnTriggerEnter(Collider other) {
		GameObject otherObj = other.gameObject;
		if (otherObj == null || otherObj.tag != "Role" || (bulletData.Type != BulletType.Heal && bulletData.Id == otherObj.name)) {
			return;
		}
		if (hitedOtherPosition.ContainsKey(otherObj.name)) {
			return;
		}
		Vector3 newToPosition = otherObj.transform.position;
		if (!isDied) {
			hitedOtherPosition.Add(otherObj.name, newToPosition);
			bulletData.AppendAffectedRoleId(otherObj.name, newToPosition);
			Messenger.Broadcast<BulletData>(NotifyTypes.BulletHited, bulletData);
		}
		bulletData.Life--;
		isDied = bulletData.Life <= 0;
		if (isDied) {
			bulletData.ToPosition = newToPosition;
		}
	}
}
