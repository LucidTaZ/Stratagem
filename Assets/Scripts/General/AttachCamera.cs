using UnityEngine;
using UnityEngine.Networking;

public class AttachCamera : NetworkBehaviour {
	public Transform AttachmentPoint;

	CameraBehavior[] cbs;

	bool quitting = false;

	void Awake () {
		cbs = GameObject.FindObjectsOfType<CameraBehavior>();
		Debug.Assert(cbs.Length > 0);
	}

	public void PerformAttach () {
		foreach (CameraBehavior cb in cbs) {
			cb.AttachTo(AttachmentPoint);
		}
	}

	void OnApplicationQuit () {
		quitting = true;
	}

	void OnDestroy () {
		// Also gets called on application quit.
		if (!quitting && hasAuthority) {
			detach();
		}
	}

	void detach () {
		foreach (CameraBehavior cb in cbs) {
			cb.Detach();
		}
	}
}
