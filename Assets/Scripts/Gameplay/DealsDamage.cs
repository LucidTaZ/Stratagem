using UnityEngine;

public class DealsDamage : MonoBehaviour {
	public int Damage = 1;

	public float SecondsBetweenTicks = 1.0f;

	public bool FriendlyFire = false;

	float cooldown = 0;

	Team shotByTeam;

	public void SetShooter (GameObject shooter) {
		BelongsToTeam btt = shooter.GetComponent<BelongsToTeam>();
		if (btt != null) {
			shotByTeam = btt.team;
		}
	}

	void Update () {
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		}
	}

	void OnCollisionEnter (Collision collision) {
		dealDamageIfApplicable(collision.gameObject);
	}

	void OnTriggerEnter (Collider other) {
		dealDamageIfApplicable(other.gameObject);
	}

	void OnTriggerStay (Collider other) {
		dealDamageIfApplicable(other.gameObject);
	}

	void dealDamageIfApplicable (GameObject other) {
		Hitpoints hitpoints;
		if ((hitpoints = other.GetComponent<Hitpoints>()) != null) {
			BelongsToTeam btt = other.GetComponent<BelongsToTeam>();
			if (FriendlyFire || btt == null || !shotByTeam.IsFriendsWith(btt.team)) {
				dealDamageByTickTime(hitpoints);
			}
		}
	}

	void dealDamageByTickTime (Hitpoints hitpoints) {
		if (cooldown <= 0) {
			hitpoints.Decrease(Damage);
			cooldown = SecondsBetweenTicks;
		}
	}
}
