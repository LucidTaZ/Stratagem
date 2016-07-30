using UnityEngine;

public class AttachCamera : MonoBehaviour {
	public Transform AttachmentPoint;

	CameraBehavior cb;

	bool quitting = false;

	void Start () {
		cb = Camera.main.GetComponent<CameraBehavior>();
		Debug.Assert(cb != null);
		cb.AttachTo(AttachmentPoint);
	}

	void OnApplicationQuit () {
		quitting = true;
	}

	void OnDestroy () {
		// Also gets called on application quit.
		if (!quitting) {
			detach();
		}
	}
	void detach () {
		cb.Detach();
	}
}
