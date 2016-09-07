using UnityEngine;

public class RaiseTurretOnAggro : MonoBehaviour {
	public Animator AnimationController;

	FindTarget findTarget;

	void Awake () {
		findTarget = GetComponent<FindTarget>();
	}
	
	void Update () {
		bool hasTarget = findTarget.HasTarget();
		AnimationController.SetBool("IsAlert", hasTarget);
	}
}
