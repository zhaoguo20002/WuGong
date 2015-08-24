using UnityEngine;
using System.Collections;

public class CameraSizeSet : MonoBehaviour {
	Camera camera;
	void Awake() {
		Statics.InitCamera();
		camera = GetComponent<Camera>();
	}

	// Use this for initialization
	void Start () {
		camera.orthographicSize = Statics.GetResolution().y * 0.005f;
	}
}
