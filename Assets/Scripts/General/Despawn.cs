using UnityEngine;
using UnityEngine.Networking;

public class Despawn : NetworkBehaviour {

	public float TimeToLive;
	float DeathTime;

	void Start () {
		DeathTime = Time.time + TimeToLive;
	}

	void Update () {
		if (Time.time < DeathTime) {
			return;
		}
		if (!hasAuthority) {
			return;
		}
		if (NetworkServer.active) {
			NetworkServer.Destroy(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
