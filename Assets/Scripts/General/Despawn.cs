using UnityEngine;

public class Despawn : MonoBehaviour {

	public float TimeToLive;
	float DeathTime;

	void Start () {
		DeathTime = Time.time + TimeToLive;
	}

	void Update () {
		if (Time.time < DeathTime) {
			return;
		}
		Destroy(gameObject);
	}
}
