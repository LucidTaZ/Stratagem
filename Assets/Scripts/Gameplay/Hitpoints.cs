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
		if (isActuallyLocalPlayer()) {
			Camera.main.GetComponent<CameraBehavior>().Detach();
		}

		NetworkServer.Destroy(gameObject);
	}

	bool isActuallyLocalPlayer () {
		return CompareTag("Player") && hasAuthority;
	}
}
