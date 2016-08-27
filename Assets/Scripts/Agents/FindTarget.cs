using UnityEngine;
using UnityEngine.Networking;

public class FindTarget : NetworkBehaviour {
	public GameObject CurrentTarget;

	TargetDomain domain;

	void Awake () {
		KnowsDomain kd = GetComponent<KnowsDomain>();
		if (kd != null) {
			domain = kd.Domain;
		}
	}

	public void SetDomain (TargetDomain referenceDomain) {
		domain = referenceDomain;
	}

	public void ForgetCurrentTarget () {
		CurrentTarget = null;
	}

	void Update () {
		if (CurrentTarget == null || CurrentTarget.Equals(null) || !CurrentTarget.activeInHierarchy) {
			CurrentTarget = findNewTarget();
		}
	}

	GameObject findNewTarget () {
		if (domain == null) {
			// Domain can be set later in some cases
			return null;
		}
		return domain.findTarget();
	}
}
