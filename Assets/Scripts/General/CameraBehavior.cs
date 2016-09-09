using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
	public float ResetTime = 1f;
	public bool FollowRotation = true;
	public bool FixAltitude = false;

	Vector3 initialPosition;
	Quaternion initialRotation;

	void Start () {
		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}

	void LateUpdate () {
		if (!FollowRotation) {
			transform.rotation = initialRotation;
		}
		if (FixAltitude) {
			transform.position = new Vector3(
				transform.position.x,
				initialPosition.y,
				transform.position.z
			);
		}
	}

	public void AttachTo (Transform attachmentPoint) {
		transform.parent = attachmentPoint;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = new Vector3(1, 1, 1);
	}

	public void Detach () {
		transform.parent = null;
		StartCoroutine(performSweep(initialPosition, initialRotation));
	}

	IEnumerator performSweep (Vector3 position, Quaternion rotation) {
		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;

		for (float t = 0; t < ResetTime; t += Time.deltaTime) {
			float smoothT = Mathf.SmoothStep(0, 1, t / ResetTime);
			transform.position = Vector3.Lerp(startPosition, position, smoothT);
			transform.rotation = Quaternion.Slerp(startRotation, rotation, smoothT);
			yield return 0;
		}
	}
}
