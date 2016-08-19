using UnityEngine;
using UnityEngine.Networking;

public class Hitpoints : NetworkBehaviour {

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
}
