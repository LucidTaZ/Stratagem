using UnityEngine;
using UnityEngine.Networking;

public class Approach : NetworkBehaviour {
	public float ApproachDistanceMin = 0;
	float approachDistanceMinSq;

	GameObject currentTarget;

	Vector3 basePosition;
	NavMeshAgent nav;

	FindTarget findTarget;

	void Start () {
		basePosition = transform.position;
		approachDistanceMinSq = ApproachDistanceMin * ApproachDistanceMin;

		nav = GetComponent<NavMeshAgent>();
		findTarget = GetComponent<FindTarget>();

		Debug.Assert(nav != null);
		Debug.Assert(findTarget != null);
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}
		if (findTarget.CurrentTarget == null) {
			headHome();
		} else {
			followCurrentTarget();
			approach();
		}
	}

	void followCurrentTarget () {
		if (findTarget.CurrentTarget != null) {
			nav.SetDestination(findTarget.CurrentTarget.transform.position);
		}
	}

	void approach () {
		if (findTarget.CurrentTarget == null) {
			nav.Resume();
			return;
		}

		bool closeEnough = (findTarget.CurrentTarget.transform.position - transform.position).sqrMagnitude < approachDistanceMinSq;
		if (closeEnough) {
			nav.Stop();
		} else {
			nav.Resume();
		}
	}

	void headHome () {
		nav.SetDestination(basePosition);
		nav.Resume();
	}
}
