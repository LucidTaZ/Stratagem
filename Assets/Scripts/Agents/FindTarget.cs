using UnityEngine;
using UnityEngine.Networking;

public class FindTarget : NetworkBehaviour {
	public GameObject CurrentTarget;

	[SyncVar]
	TargetDomain domain;

	void Start () {
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
		return domain.findTarget();
	}
}
