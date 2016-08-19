using UnityEngine;
using UnityEngine.Networking;

public class AttachCamera : NetworkBehaviour {
	public Transform AttachmentPoint;

	CameraBehavior cb;

	bool quitting = false;

	void Awake () {
		cb = Camera.main.GetComponent<CameraBehavior>();
		Debug.Assert(cb != null);
	}

	public void PerformAttach () {
		cb.AttachTo(AttachmentPoint);
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
		cb.Detach();
	}
}
