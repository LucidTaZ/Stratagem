using UnityEngine;

public class FindTargetByCapacityController : MonoBehaviour {
	Inventory inventory;
	FindTarget findTarget;

	FindTarget.Mode originalTargetingMode;

	void Awake () {
		inventory = GetComponent<Inventory>();
		findTarget = GetComponent<FindTarget>();

		Debug.Assert(inventory != null);
		Debug.Assert(findTarget != null);

		originalTargetingMode = findTarget.TargetingMode;
	}

	void Update () {
		// When full, lose the target (Approach script will then send us home)
		// and wait there until the inventory is completely empty.
		if (!inventory.HasSpace()) {
			findTarget.TargetingMode = FindTarget.Mode.NONE;
			findTarget.ForceForgetTarget();
		}
		if (inventory.Contents.Count == 0) {
			findTarget.TargetingMode = originalTargetingMode;
		}
	}
}
