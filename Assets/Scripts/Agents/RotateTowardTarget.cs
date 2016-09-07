using UnityEngine;

public class RotateTowardTarget : MonoBehaviour {
	public float SlerpSpeed;

	FindTarget findTarget;
	Quaternion initialRotation;

	void Awake () {
		findTarget = GetComponentInParent<FindTarget>();
		Debug.Assert(findTarget != null);

		initialRotation = transform.rotation;
	}

	void Update () {
		if (findTarget.HasTarget()) {
			Vector3 targetPosition = findTarget.CurrentTarget.transform.position;
			Vector3 targetDirection = targetPosition - transform.position;

			targetDirection.y = 0; // Ensure we only rotate around world y

			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				Quaternion.LookRotation(targetDirection, Vector3.up) * initialRotation,
				SlerpSpeed * Time.deltaTime
			);
		}
	}
}
