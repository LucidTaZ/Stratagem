using UnityEngine;

public class DealsDamage : MonoBehaviour {
	public int Damage = 1;

	public float SecondsBetweenTicks = 1.0f;

	float cooldown = 0;

	void Update () {
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		}
	}

	void OnCollisionEnter (Collision collision) {
		Hitpoints hitpoints;
		if (hitpoints = collision.gameObject.GetComponent<Hitpoints>()) {
			dealDamageByTickTime(hitpoints);
		}
	}

	void OnTriggerEnter (Collider other) {
		Hitpoints hitpoints;
		if (hitpoints = other.gameObject.GetComponent<Hitpoints>()) {
			dealDamageByTickTime(hitpoints);
		}
	}

	void OnTriggerStay (Collider other) {
		Hitpoints hitpoints;
		if (hitpoints = other.gameObject.GetComponent<Hitpoints>()) {
			dealDamageByTickTime(hitpoints);
		}
	}

	void dealDamageByTickTime (Hitpoints hitpoints) {
		if (cooldown <= 0) {
			hitpoints.Decrease(Damage);
			cooldown = SecondsBetweenTicks;
		}
	}
}
