using UnityEngine;
using System.Collections;
using Game;

public class Global : MonoBehaviour {
	public Transform Tk2DCamera;
	public Transform FingerGestures;
	public Transform HostController;

	void Awake () {
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(Tk2DCamera.gameObject);
		DontDestroyOnLoad(FingerGestures.gameObject);
		DontDestroyOnLoad(HostController.gameObject);
		NotifyBase.Init();
	}

	void Start() {
		QualitySettings.vSyncCount = -1;
		QualitySettings.maxQueuedFrames = 0;
		Application.targetFrameRate = 30;

		Messenger.Broadcast<string>(NotifyTypes.GoToScene, "Scene001");
	}
}
