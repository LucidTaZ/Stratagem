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
		Debug.Assert(!hasAuthority || NetworkServer.active);
		if (!hasAuthority) {
			return;
		}
		NetworkServer.Destroy(gameObject);
	}
}
