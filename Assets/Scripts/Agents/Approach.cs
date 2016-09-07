using UnityEngine;
using UnityEngine.Networking;

public class Approach : NetworkBehaviour {
	public float ApproachDistanceMin = 0;
	float approachDistanceMinSq;

	public float FollowDistanceMax = Mathf.Infinity;
	float followDistanceMaxSq;

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
		followDistanceMaxSq = FollowDistanceMax * FollowDistanceMax;
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}
		if (!findTarget.HasTarget()) {
			headHome();
		} else {
			if (targetIsInRange()) {
				followCurrentTarget();
				approach();
				findTarget.VoteKeepTarget();
			} else {
				// TODO: Find a way to prevent kiting, use the target domain?
				findTarget.VoteForgetTarget();
			}
		}
	}

	void followCurrentTarget () {
		RpcSetNavDestination(findTarget.CurrentTarget.transform.position);
	}

	bool targetIsInRange () {
		return (findTarget.CurrentTarget.transform.position - transform.position).sqrMagnitude < followDistanceMaxSq;
	}

	void approach () {
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
