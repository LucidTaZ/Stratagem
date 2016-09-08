using UnityEngine;
using UnityEngine.Networking;

public class Hitpoints : NetworkBehaviour {
	public bool AllowCollisionsInChildren = false;
	public bool AllowCollisionsInTriggers = true;

	public int Initial = 5;

	int current;

	void Start () {
		current = Initial;
	}

	public void Decrease (int delta) {
		current -= delta;
		if (current <= 0) {
			Die();
		}
	}

	void Die () {
		Debug.Assert(NetworkServer.active);

		if (CompareTag("Player")) {
			RpcDetachCamera();
		}

		NetworkServer.Destroy(gameObject);
	}

	[ClientRpc]
	public void RpcDetachCamera () {
		if (hasAuthority) {
			// Only for the local player
			Camera.main.GetComponent<CameraBehavior>().Detach();
		}
	}

	public static Hitpoints FindHitpointsComponent (GameObject hitGameObject) {
		// Find the Hitpoints component of a hit gameobject
		// It may be located on a parent piece (or in the future perhaps on a child piece)

		Hitpoints hitpoints = hitGameObject.GetComponent<Hitpoints>();
		if (hitpoints != null) {
			return hitpoints;
		}
		Hitpoints[] potentialParentHitpoints = hitGameObject.GetComponentsInParent<Hitpoints>();
		foreach (Hitpoints parentHitpoints in potentialParentHitpoints) {
			if (parentHitpoints.AllowCollisionsInChildren) {
				return parentHitpoints;
			}
		}
		return null;
	}
}
