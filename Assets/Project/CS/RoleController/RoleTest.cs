using UnityEngine;
using System.Collections;

public class RoleTest : MonoBehaviour {
	tk2dSpriteAnimator ani;
	public Material lineMat;
	// Use this for initialization
	void Start () {
		ani = GetComponent<tk2dSpriteAnimator>();
		ani.AnimationCompleted = completeCallback;
	}

	void completeCallback(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip) {
		Debug.LogWarning(sprite.name + "," + clip.name);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			ani.Play("left");
		}
		else if (Input.GetKeyDown(KeyCode.W)) {
			ani.Play("up");
		}
		else if (Input.GetKeyDown(KeyCode.D)) {
			ani.Play("right");
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			ani.Play("down");
		}
		else if (Input.GetKeyDown(KeyCode.Space)) {
			ani.Play("round");
		}
	}

}
