using UnityEngine;
using UnityEngine.Networking;

public class FindTarget : NetworkBehaviour {
	public GameObject CurrentTarget;

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
		return domain.findTarget();
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
