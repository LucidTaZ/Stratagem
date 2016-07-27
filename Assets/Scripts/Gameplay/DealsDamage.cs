using UnityEngine;

public class DealsDamage : MonoBehaviour {
	public int Damage = 1;

	void OnCollisionEnter (Collision collision) {
		Hitpoints hitpoints;
		if (hitpoints = collision.gameObject.GetComponent<Hitpoints>()) {
			hitpoints.Decrease(Damage);
		}
	}
}
