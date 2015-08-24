using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMain : MonoBehaviour {
	public Animator weaponAin;
	// Use this for initialization
	void Start () {
		weaponAin.speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if (GUI.Button(new Rect(200, 80, 60, 50), "left")) {
			weaponAin.Play("StickAttack0_left");
		}
		if (GUI.Button(new Rect(270, 20, 60, 50), "up")) {
			weaponAin.Play("StickAttack0_up");
		}
		if (GUI.Button(new Rect(340, 80, 60, 50), "right")) {
			weaponAin.Play("StickAttack0_right");
		}
		if (GUI.Button(new Rect(270, 120, 60, 50), "down")) {
			weaponAin.Play("StickAttack0_down");
		}
	}
}
