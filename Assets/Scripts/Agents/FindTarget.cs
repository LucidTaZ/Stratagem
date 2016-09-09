using UnityEngine;
using UnityEngine.Networking;

public class FindTarget : NetworkBehaviour {
	public enum Mode {
		NONE, // To disable target finding, e.g. when the worker inventory is full
		CLOSEST_TO_DOMAIN,
		CLOSEST_TO_SELF
	}

	public GameObject CurrentTarget;

	public Mode TargetingMode = Mode.CLOSEST_TO_DOMAIN;

	TargetDomain domain;

	bool voteForgetTarget;
	bool voteKeepTarget;

	void Awake () {
		KnowsDomain kd = GetComponent<KnowsDomain>();
		if (kd != null) {
			domain = kd.Domain;
		}
	}

	public void SetDomain (TargetDomain referenceDomain) {
		domain = referenceDomain;
	}

	public bool HasTarget () {
		return CurrentTarget != null;
	}

	// Inform the script that the target preferably is dropped
	public void VoteForgetTarget () {
		voteForgetTarget = true;
	}

	// Inform the script that the target should be kept
	public void VoteKeepTarget () {
		voteKeepTarget = true;
	}

	public void ForceForgetTarget () {
		forgetCurrentTarget();
	}

	void Update () {
		if (!NetworkServer.active) {
			return;
		}
		if (CurrentTarget == null || CurrentTarget.Equals(null) || !CurrentTarget.activeInHierarchy) {
			CurrentTarget = findNewTarget();
			if (CurrentTarget != null) {
				// Inform the clients of the target, for client-side aggro animations.
				RpcSetTarget(CurrentTarget.GetComponent<NetworkIdentity>().netId);
			}
		}
	}

	void LateUpdate () {
		if (!NetworkServer.active) {
			return;
		}
		if (!voteKeepTarget && voteForgetTarget) {
			forgetCurrentTarget();
		}
		voteForgetTarget = false;
		voteKeepTarget = false;
	}

	void forgetCurrentTarget () {
		CurrentTarget = null;
		RpcUnsetTarget();
	}

	GameObject findNewTarget () {
		if (domain == null) {
			// Domain can be set later in some cases
			return null;
		}
		if (TargetingMode == Mode.NONE) {
			return null;
		} else if (TargetingMode == Mode.CLOSEST_TO_DOMAIN) {
			return domain.findTarget();
		} else if (TargetingMode == Mode.CLOSEST_TO_SELF) {
			return domain.findTarget(transform.position);
		}
		Debug.LogError("Cannot handle TargetingMode " + TargetingMode);
		return null;
	}

	[ClientRpc]
	public void RpcSetTarget (NetworkInstanceId netId) {
		CurrentTarget = ClientScene.FindLocalObject(netId);
	}

	[ClientRpc]
	public void RpcUnsetTarget () {
		CurrentTarget = null;
	}
}
