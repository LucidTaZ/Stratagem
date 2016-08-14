using UnityEngine;
using UnityEngine.Networking;

public class DestroyOnImpact : NetworkBehaviour {
	void OnCollisionEnter (Collision collision) {
		if (!hasAuthority) {
			// Don't allow local collision to destroy models, even locally. Shooting many bullets can make them appear in the same spot for clients.
			return;
		}
		if (NetworkServer.active) {
			NetworkServer.Destroy(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
