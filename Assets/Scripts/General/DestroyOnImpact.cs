using UnityEngine;
using UnityEngine.Networking;

public class DestroyOnImpact : NetworkBehaviour {
	void OnCollisionEnter (Collision collision) {
		Debug.Assert(!hasAuthority || NetworkServer.active);
		if (!hasAuthority) {
			// Don't allow local collision to destroy models, even locally. Shooting many bullets can make them appear in the same spot for clients.
			return;
		}
		NetworkServer.Destroy(gameObject);
	}
}
