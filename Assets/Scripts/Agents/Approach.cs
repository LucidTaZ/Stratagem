using UnityEngine;
using UnityEngine.Networking;

public class Approach : NetworkBehaviour {
	public float ApproachDistanceMin = 0;
	float approachDistanceMinSq;

	Vector3 basePosition;

	NavMeshAgent nav; // nav instructions are replicated via ClientRpc to have client-side movement prediction with server authority

	FindTarget findTarget;

	void Awake () {
		nav = GetComponent<NavMeshAgent>();
		findTarget = GetComponent<FindTarget>();

		Debug.Assert(nav != null);
		Debug.Assert(findTarget != null);
	}

	void Start () {
		basePosition = transform.position;
		approachDistanceMinSq = ApproachDistanceMin * ApproachDistanceMin;
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
			RpcSetNavDestination(findTarget.CurrentTarget.transform.position);
		}
	}

	void approach () {
		if (findTarget.CurrentTarget == null) {
			RpcNavResume();
			return;
		}

		bool closeEnough = (findTarget.CurrentTarget.transform.position - transform.position).sqrMagnitude < approachDistanceMinSq;
		if (closeEnough) {
			RpcNavStop();
		} else {
			RpcNavResume();
		}
	}

	void headHome () {
		RpcSetNavDestinationAndResume(basePosition);
	}

	[ClientRpc]
	public void RpcSetNavDestination (Vector3 position) {
		nav.SetDestination(position);
	}

	[ClientRpc]
	public void RpcSetNavDestinationAndResume (Vector3 position) {
		nav.SetDestination(position);
		nav.Resume();
	}

	[ClientRpc]
	public void RpcNavResume () {
		nav.Resume();
	}

	[ClientRpc]
	public void RpcNavStop () {
		nav.Stop();
	}
}
