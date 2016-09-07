using UnityEngine;

public class RotateTowardTarget : MonoBehaviour {
	public Transform Bone;

	public float SlerpSpeed;

	FindTarget findTarget;
	Quaternion initialRotation;

	void Awake () {
		findTarget = GetComponent<FindTarget>();
		Debug.Assert(findTarget != null);

		initialRotation = Bone.rotation;
	}

	void Update () {
		if (findTarget.HasTarget()) {
			Vector3 targetPosition = findTarget.CurrentTarget.transform.position;
			Vector3 targetDirection = targetPosition - Bone.position;
			targetDirection.y = 0; // Ensure we only rotate around world y

			Bone.rotation = Quaternion.Slerp(
				Bone.rotation,
				Quaternion.LookRotation(targetDirection, Vector3.up) * initialRotation,
				SlerpSpeed * Time.deltaTime
			);
		}
	}
}
